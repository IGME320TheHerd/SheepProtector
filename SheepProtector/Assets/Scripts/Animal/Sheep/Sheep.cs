using UnityEngine;

/// <summary>
/// The different states the sheep can be in.
/// Flee - the sheep running away from whatever scared it.
/// Wander - the sheep randomly wandering around its current location.
/// Still - the sheep not moving around at all.
/// </summary>
enum SheepState { 
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

    private float randomWanderTimer = 6.0f;

    private float stopWanderTimer = 5.0f;

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    private void Start()
    {
        currentState = SheepState.Still;

        // If the sheep's reference to the player game object is null, grab it.
        if (player == null)
        {
            player = FindAnyObjectByType<Sheepdog>().gameObject;
        }

        // Set the thirst level of the sheep to 0. THIS IS CURRENTLY UNUSED!
        //thirst = 0.0f;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>

    /// <summary>
    /// How the sheep should react when the dog barks.
    /// </summary>
    /// <param name="callBackContext"></param>
    public override void BarkReaction()
    {
        fleeTarget = player;
        ToFleeState(fleeTarget.transform.position);
    }

    /// <summary>
    /// What should happen if the sheep were to die.
    /// </summary>
    protected override void Die()
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
        switch(currentState)
        {
            case SheepState.Still:
                randomWanderTimer -= Time.deltaTime;

                if (randomWanderTimer <= 0.0f)
                {
                    float rng = Random.Range(0, 100);
                    if (rng <= 30.0f)
                    {
                        ToWanderState();
                    }
                    randomWanderTimer = 6.0f;
                }
                break;
            case SheepState.Wander:
                stopWanderTimer -= Time.deltaTime;

                if (stopWanderTimer <= 0.0f)
                {
                    ToStillState();
                }
                acceleration += Wander(2.0f, 5.0f, 0.5f);
                break;
            case SheepState.Flee:
                float playerdist = Vector3.Distance(transform.position, fleeTarget.transform.position);
                acceleration += Flee(fleeTarget) * 200.0f / playerdist / leaveFleeDist;
                break;
        }

        RaycastHit hit;
        RaycastHit hit2;
        RaycastHit hit3;

        float hitdist = 5.0f;

        if (Physics.Raycast(transform.position, velocity.normalized, out hit, hitdist))
        {
            acceleration += hit.normal * wallFleeWeight * ((1 / hit.distance) / hitdist);
        }

        if (Physics.Raycast(transform.position, Vector3.Cross(velocity.normalized, transform.up), out hit2, hitdist))
        {
            acceleration += hit2.normal * wallFleeWeight * ((1 / hit2.distance) / hitdist);
        }

        if (Physics.Raycast(transform.position, Vector3.Cross(velocity.normalized, -transform.up), out hit3, hitdist))
        {
            acceleration += hit3.normal * wallFleeWeight * ((1/hit3.distance) / hitdist);
        }


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
        maxSpeed = 2.0f;
        randomWanderTimer = 5.0f;
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
    /// How the sheep should react upon a collision.
    /// </summary>
    /// <param name="other"> The other game object in the collision. </param>
    private void OnCollisionStay(Collision collision)
    {
        // If the sheep hits a wall, move it away from that wall.
        if (collision.collider.GetType() == typeof(BoxCollider) && collision.collider.gameObject != player)
        {
            if (currentState == SheepState.Flee || currentState == SheepState.Wander)
            {
                //velocity = 
               //acceleration += Flee(collision.GetContact(0).point);
            }
        }
    }

    protected override Vector3 CalcSteering()
    {
        return Vector3.zero;
    }
}
