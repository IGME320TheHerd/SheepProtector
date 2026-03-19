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


    private float randomWanderTimer;

    private float stopWanderTimer;
    // True if the sheep is fleeing from the sheepdog for being too close, false if not.
    private bool tooClose;

    /// <summary>
    /// Make tooClose public;
    /// True if the sheep is fleeing from the sheepdog for being too close, false if not.
    /// </summary>
    public bool TooClose
    {
        get {  return !tooClose; }
        set { tooClose = value; }
    }


    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    private void Start()
    {
        randomWanderTimer = stillTime;
        stopWanderTimer = wanderLength;
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
        ToFleeState(fleeTarget.transform.position);
    }

    /// <summary>
    /// What should happen if the sheep were to die.
    /// </summary>
    public override void Die()
    {

    }

    private void Update()
    {
        acceleration = Vector3.zero;
        Movement();

        acceleration.y = 0.0f;
    }

    public void LeaveBark()
    {
        ToStillState();
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
                float playerdist = Vector3.Distance(transform.position, fleeTarget.transform.position);
                acceleration += Flee(fleeTarget) * 200.0f / playerdist / leaveFleeDist;
                break;
        }

        RaycastHit hit;
        RaycastHit hit2;
        RaycastHit hit3;

        float hitdist = 5.0f;

        // If the sheep is heading towards a wall, have it start moving away from it.
        if (Physics.Raycast(transform.position, velocity.normalized, out hit, hitdist))
        {
            acceleration += hit.normal * wallFleeWeight * ((1 / hit.distance) / hitdist);
            stopWanderTimer -= wanderLength * 0.01f;
        }

        // If the sheep is heading towards a wall, have it start moving away from it.
        if (Physics.Raycast(transform.position, Vector3.Cross(velocity.normalized, transform.up), out hit2, hitdist))
        {
            acceleration += hit2.normal * wallFleeWeight * ((1 / hit2.distance) / hitdist);
            stopWanderTimer -= wanderLength * 0.01f;
        }

        // If the sheep is heading towards a wall, have it start moving away from it.
        if (Physics.Raycast(transform.position, Vector3.Cross(velocity.normalized, -transform.up), out hit3, hitdist))
        {
            acceleration += hit3.normal * wallFleeWeight * ((1 / hit3.distance) / hitdist);
            stopWanderTimer -= wanderLength * 0.01f;
        }

        // Draw the rays with gizmos active to show the direction of the sheep.
        Debug.DrawRay(transform.position, velocity.normalized * hitdist, Color.red);
        Debug.DrawRay(transform.position, Vector3.Cross(velocity.normalized * hitdist, transform.up), Color.blue);
        Debug.DrawRay(transform.position, Vector3.Cross(velocity.normalized * hitdist, -transform.up), Color.green);

    }

    /// <summary>
    /// How the sheep should transition everything to the flee state.
    /// </summary>
    /// <param name="targetPos"> The position the sheep should flee from, usually an animal's position. </param>
    private void ToFleeState(Vector3 targetPos)
    {
        maxSpeed = 7.0f;
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
            && other.GetType() != typeof(SphereCollider)
            && currentState != SheepState.Flee)
        {
            tooClose = true;
            fleeTarget = other.gameObject;
            ToFleeState(fleeTarget.transform.position);
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
            && other.GetType() != typeof(SphereCollider)
            && tooClose)
        {
            tooClose = false;
            ToStillState();
        }
    }

    protected override Vector3 CalcSteering()
    {
        return Vector3.zero;
    }
}
