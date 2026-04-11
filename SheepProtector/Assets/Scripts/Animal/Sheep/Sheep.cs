using UnityEngine;

/// <summary>
/// The different states the sheep can be in.
/// Flee - the sheep running away from whatever scared it.
/// Wander - the sheep randomly wandering around its current location.
/// Still - the sheep not moving around at all.
/// </summary>
enum SheepState
{
    Flee,
    Wander,
    Still
}

public class Sheep : Animal
{
    // The sheep state the sheep is currently in.
    private SheepState currentState;

    // The thirst meter for the sheep. THIS IS CURRENTLY UNUSED!
    //private float thirst;

    // Fields for sheep movement.
    [SerializeField] private Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);

    // When fleeing, this is the target the sheep flees from (for right now, it should be the player).
    [SerializeField] private GameObject fleeTarget;
    [SerializeField] private float maxBarkTimer = 5.0f;
    private float barkTimer = 0.0f;
    bool timeLowered = false;
    [SerializeField] private int barkCount = 0;

    // When wandering, this is the target the sheep wanders to.
    [SerializeField] private Vector3 wanderPos;

    // The player's game object.
    [SerializeField] private GameObject player;

    // The weight of the sheep moving away from a wall.
    [SerializeField] private float wallFleeWeight = 0.5f;

    // If the sheep gets this far away from a flee target, it will no longer be fleeing from that target.
    [SerializeField] private float leaveFleeDist = 10.0f;

    // How long between rng checks for sheep attempting to wander
    public float stillTime = 3.0f;

    // How long sheep should wander
    public float wanderLength = 15.0f;

    // How fast sheep goes during wander
    public float wanderSpeed = 6.0f;

    private bool outOfRange = false;

    private float rangeWaitTimer = 0.0f;


    private float randomWanderTimer;

    private float stopWanderTimer;
    // True if the sheep is fleeing from the sheepdog for being too close, false if not.
    private bool tooClose; // For if the sheep is not already fleeing
    private bool inRangeBarkCheck; // For if the sheep is fleeing from the dog barking and the dog ends in range of the sheep (keeps the sheep going).
    private float closeTimer;
    [SerializeField]private float maxCloseTimer = 5.0f;
    private SpriteRenderer sr;

    /// <summary>
    /// Make tooClose public;
    /// True if the sheep is fleeing from the sheepdog for being too close, false if not.
    /// </summary>
    public bool TooClose
    {
        get {  return tooClose; }
        set { tooClose = value; }
    }

    /// <summary>
    /// Make inRangeBarkCheck public;
    /// True if the sheep is fleeing from the sheepdog for being too close, false if not. Used for if the dog has barked.
    /// </summary>
    public bool InRangeBarkCheck
    {
        get { return inRangeBarkCheck; }
        set { inRangeBarkCheck = value; }
    }

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        randomWanderTimer = stillTime;
        stopWanderTimer = wanderLength;
        closeTimer = 0;
        currentState = SheepState.Still;
        tooClose = false;

        // If the sheep's reference to the player game object is null, grab it.
        if (player == null)
        {
            player = FindAnyObjectByType<Sheepdog>().gameObject;
        }

        // Set the thirst level of the sheep to 0. THIS IS CURRENTLY UNUSED!
        //thirst = 0.0f;
    }

    /// <summary>
    /// How the sheep should react when the dog barks.
    /// </summary>
    public override void BarkReaction()
    {
        fleeTarget = player;
        barkCount++;

        // If the player is spamming the bark button, penalize them by making the max bark timer decrease by 2 seconds.
        if (barkCount >= 5)
        {
            barkTimer = maxBarkTimer - 2.0f;
            timeLowered = true;
        }

        // If the player is not spamming the bark button, set the bark timer to the max.
        else
        {
            barkTimer = maxBarkTimer;
            timeLowered = false;
        }

        maxSpeed = 13f;
        ToFleeState(fleeTarget.transform.position);
    }

    /// <summary>
    /// What should happen if the sheep were to die.
    /// </summary>
    public override void Die()
    {
        // Get the game manager
        GameManager gameOverCaller = GameObject.FindAnyObjectByType<GameManager>();

        // If a game manager was not found, create a new one.
        if (gameOverCaller == null)
        {
            gameOverCaller = new GameManager();
        }

        // Set the state of the game over manager to the game over state.
        gameOverCaller.SetState(4);
    }

    private void Update()
    {
        acceleration = Vector3.zero;
        Movement();

        if (rangeWaitTimer > 0.0f)
        {
            rangeWaitTimer -= Time.deltaTime;
        }

        if (outOfRange && rangeWaitTimer <= 0.0f)
        {
            ToStillState();
            outOfRange = false;
        }

        acceleration.y = 0.0f;
    }

    /// <summary>
    /// If the sheep gets out of the sheepdog's bark range, have a small countdown go off, then set the sheep to the still state.
    /// </summary>
    public void LeaveBark()
    {
        rangeWaitTimer = 0.2f;
        outOfRange = true;
    }

    /// <summary>
    /// How the sheep should move depending on its current state.
    /// </summary>
    protected override void Movement()
    {
        switch (currentState)
        {
            // If the sheep is in the still state, have it wander at random.
            case SheepState.Still:
                randomWanderTimer -= Time.deltaTime;

                // If the random wander timer reaches 0, check to see if the sheep should wander.
                if (randomWanderTimer <= 0.0f)
                {
                    // If the sheep passes a random number check, have it wander to a random nearby point.
                    float rng = Random.Range(0, 100);
                    if (rng <= 40.0f)
                    {
                        ToWanderState();
                    }

                    randomWanderTimer = stillTime;
                }
                break;

            // If the sheep is wandering, have the acceleration face towards the specific target.
            case SheepState.Wander:
                stopWanderTimer -= Time.deltaTime;

                // If the sheep wanders for too long, have it stop wandering.
                if (stopWanderTimer <= 0.0f)
                {
                    ToStillState();
                }
                acceleration += Wander(2.0f, 5.0f, 0.5f);
                break;

            // If the sheep is fleeing, have the acceleration face away the specific target.
            case SheepState.Flee:
                // If the sheep is fleeing for the dog barking, have it stop fleeing after a bit.
                if (barkTimer > 0.0f)
                {
                    barkTimer -= Time.deltaTime;// * barkCount;

                    // Reset the bark counter if the player is no longer spamming the bark button. 
                    if ((barkTimer <= maxBarkTimer - 1.0f && !timeLowered) || 
                        (barkTimer <= maxBarkTimer - 3.0f && timeLowered))
                    {
                        barkCount = 0;
                    }

                    // If the bark timer ends, put the sheep in the still state if the sheep is far enough away from the sheepdog.
                    if (barkTimer <= 0.0f)
                    {       
                        // If the sheep is not far enough away from the sheepdog, have it keep fleeing.
                        if (inRangeBarkCheck)
                        {
                            tooClose = true;
                            closeTimer = maxCloseTimer;
                        }
                        // If the sheep is far enough away from the sheepdog, have it stop fleeing.
                        else
                        {
                            ToStillState();
                        }

                        barkCount = 0;
                    }
                }

                // If the sheep has not flee'd away from the dog for enough time for being too close, have it keep fleeing.
                else if (closeTimer > 0.0f && !tooClose)
                {
                    closeTimer -= Time.deltaTime;
                    if (closeTimer <= 0.0f)
                    {
                        ToStillState();
                    }

                }
                float playerdist = Vector3.Distance(transform.position, fleeTarget.transform.position);
                acceleration += Flee(fleeTarget) * 200.0f / playerdist / leaveFleeDist;
                break;
        }

        RaycastHit hit;
        RaycastHit hit2;
        RaycastHit hit3;

        float hitdist = 5.0f;

        // If the sheep is heading towards a wall, have it start moving away from it.
        if (Physics.Raycast(transform.position, velocity.normalized, out hit, hitdist, 0, QueryTriggerInteraction.Ignore))
        {
            acceleration += hit.normal * wallFleeWeight * ((1 / hit.distance) / hitdist);
            stopWanderTimer -= wanderLength * 0.01f;
        }

        // If the sheep is heading towards a wall, have it start moving away from it.
        if (Physics.Raycast(transform.position, 
            Vector3.Cross(velocity.normalized, transform.up), out hit2, hitdist, 0, QueryTriggerInteraction.Ignore))
        {
            acceleration += hit2.normal * wallFleeWeight * ((1 / hit2.distance) / hitdist);
            stopWanderTimer -= wanderLength * 0.01f;
        }

        // If the sheep is heading towards a wall, have it start moving away from it.
        if (Physics.Raycast(transform.position, 
            Vector3.Cross(velocity.normalized, -transform.up), out hit3, hitdist, 0, QueryTriggerInteraction.Ignore))
        {
            acceleration += hit3.normal * wallFleeWeight * ((1 / hit3.distance) / hitdist);
            stopWanderTimer -= wanderLength * 0.01f;
        }

        // Draw the rays with gizmos active to show the direction of the sheep.
        Debug.DrawRay(transform.position, velocity.normalized * hitdist, Color.red);
        Debug.DrawRay(transform.position, Vector3.Cross(velocity.normalized * hitdist, transform.up), Color.blue);
        Debug.DrawRay(transform.position, Vector3.Cross(velocity.normalized * hitdist, -transform.up), Color.green);

        Vector3 camForward = new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z);
        float angle = Vector3.SignedAngle(velocity.normalized, camForward, Vector3.up);

        if (angle > 5 && angle < 175)
        {
            sr.flipX = true;
        }
        else if (angle < -5 && angle > -175)
        {
            sr.flipX = false;
        }
    }

    /// <summary>
    /// How the sheep should transition everything to the flee state.
    /// </summary>
    /// <param name="targetPos"> The position the sheep should flee from, usually an animal's position. </param>
    private void ToFleeState(Vector3 targetPos)
    {
        // Change the state of the sheep.
        currentState = SheepState.Flee;
    }

    /// <summary>
    /// How the sheep should transition everything to the wander state.
    /// </summary>
    private void ToWanderState()
    {
        maxSpeed = wanderSpeed;
        stopWanderTimer = wanderLength;
        currentState = SheepState.Wander;
    }

    /// <summary>
    /// How the sheep should transition everything to the still state.
    /// </summary>
    private void ToStillState()
    {
        maxSpeed = 0.0f;
        currentState = SheepState.Still;
    }

    /// <summary>
    /// Nudge the sheep in a desired direction.
    /// </summary>
    private void Nudge()
    {

    }

    /// <summary>
    /// How the sheep should react upon an enter trigger.
    /// </summary>
    /// <param name="other"> The other game object in the trigger. </param>
    private void OnTriggerEnter(Collider other)
    {
        // If the sheepdog gets too close the sheep and the sheep is not currently fleeing,
        // have it flee from the dog until it gets far enough away.
        if (other.TryGetComponent<Sheepdog>(out Sheepdog doggo)
            && !other.isTrigger)
        {
            inRangeBarkCheck = true;
            if (currentState != SheepState.Flee || closeTimer > 0)
            {
                tooClose = true;
                fleeTarget = other.gameObject;
                maxSpeed = 7f;
                closeTimer = maxCloseTimer;
                ToFleeState(fleeTarget.transform.position);
            }
            
        }
    }

    /// <summary>
    /// How the sheep should react upon an exit trigger.
    /// </summary>
    /// <param name="other"> The other game object in the trigger. </param>
    private void OnTriggerExit(Collider other)
    {
        // If the sheepdog gets too close the sheep and the sheep is not currently fleeing,
        // have it flee from the dog until it gets far enough away.
        if (other.TryGetComponent<Sheepdog>(out Sheepdog doggo)
            && !other.isTrigger
            && inRangeBarkCheck)
        {
            if (inRangeBarkCheck && !tooClose)
            {
                closeTimer = 0;
            }
            tooClose = false;
            inRangeBarkCheck = false;
        }
    }

    protected override Vector3 CalcSteering()
    {
        return Vector3.zero;
    }
}
