using System.Threading;
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
    [SerializeField] private float maxSpeed = 0.1f;
    [SerializeField] private Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);

    // When fleeing, this is the target the sheep flees from (for right now, it should be the player).
    [SerializeField] private GameObject fleeTarget;

    // When wandering, this is the target the sheep wanders to.
    [SerializeField] private Vector3 wanderPos;

    [SerializeField] private GameObject Player;


    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    private void Start()
    {
        currentState = SheepState.Still;
        Player.GetComponent<Sheepdog>().barkReactors.Add(this);

        // Set the thirst level of the sheep to 0. THIS IS CURRENTLY UNUSED!
        //thirst = 0.0f;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void FixedUpdate()
    {
        // If the velocity in the x direction is greater than the max speed, reduce it to the max speed.
        if (velocity.x > maxSpeed)
        {
            velocity.x = maxSpeed;
        }

        // If the velocity in the z direction is greater than the max speed, reduce it to the max speed.
        if (velocity.z > maxSpeed)
        {
            velocity.z = maxSpeed;
        }

        GetComponent<Rigidbody>().linearVelocity -= Physics.gravity;

        Movement();
    }

    /// <summary>
    /// How the sheep should react when the dog barks.
    /// </summary>
    /// <param name="callBackContext"></param>
    public override void BarkReaction()//(ContextCallback callBackContext)
    {
        //fleeTarget = GameObject.Find("Player");
        fleeTarget = Player;
        ToFleeState(fleeTarget.transform.position);
    }

    /// <summary>
    /// What should happen if the sheep were to die.
    /// </summary>
    protected override void Die()
    {
        
    }

    /// <summary>
    /// How the sheep should move depending on its current state.
    /// </summary>
    protected override void Movement()
    {
        switch(currentState)
        {
            case SheepState.Still:
                break;
            case SheepState.Wander:
                    Vector3 newPosition = Vector3.MoveTowards(transform.position, wanderPos, maxSpeed);
                    newPosition.y = transform.position.y;
                    GetComponent<Rigidbody>().MovePosition(newPosition);
                
                // If the sheep has reached its wander destination, put it in the still state.
                if (transform.position.x == wanderPos.x && transform.position.z == wanderPos.z)
                {
                    ToStillState();
                }
                    break;
            case SheepState.Flee:
                Vector3 nextPosition = transform.position;

                // Update the position of the sheep based on the velocity and direction of the flee target.
                nextPosition = new Vector3(nextPosition.x - (velocity.x * direction.x),
                    nextPosition.y,
                    nextPosition.z - (velocity.z * direction.z));
                GetComponent<Rigidbody>().MovePosition(nextPosition);
                break;
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

        // Create the new direction the sheep should head in.
        float directionX = 0.0f;
        float directionZ = 0.0f;

        // Move the sheep away from the target in the x direction.
        directionX = -transform.position.x + fleeTarget.transform.position.x;

        // Move the sheep away from the target in the z direction.
        directionZ = -transform.position.z + fleeTarget.transform.position.z;

        // Create the direction the sheep will be moving in.
        direction = new Vector3(directionX, 0.0f, directionZ);
        direction.Normalize();
    }

    /// <summary>
    /// How the sheep should transition everything to the wander state.
    /// </summary>
    private void ToWanderState(Vector3 targetPos)
    {
        currentState = SheepState.Wander;
        wanderPos = targetPos;

        // Create the new direction the sheep should head in.
        float directionX = 0.0f;
        float directionZ = 0.0f;

        // Move the sheep toward the target in the x direction.
        directionX = transform.position.x - targetPos.x;

        // Move the sheep toward the target in the z direction.
        directionZ = +transform.position.z - targetPos.z;

        // Create the direction the sheep will be moving in.
        direction = new Vector3(directionX, 0.0f, directionZ);
        direction.Normalize();
    }

    /// <summary>
    /// How the sheep should transition everything to the still state.
    /// </summary>
    private void ToStillState()
    {
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
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("AAA");
        //if (other.collider.GetType() == typeof(SphereCollider))
        //{
        //    Player.GetComponent<Sheepdog>().barkReactors.Add(this);
        //}
        // If the sheep hits a wall, move it away from that wall.
        if (other.GetComponent<Collider>().GetType() == typeof(BoxCollider) && other.gameObject != Player)
        {
            if (currentState == SheepState.Flee || currentState == SheepState.Wander)
            {
                Vector3 newPosition = new Vector3(transform.position.x + (direction.x * 3),
                    transform.position.y,
                    transform.position.z + (direction.z * 3));
                ToWanderState(newPosition);
            }
        }
    }

    /// <summary>
    /// How the sheep should react upon leaving a collision.
    /// </summary>
    /// <param name="other"> The other game object in the ending collision. </param>
    //public void OnCollisionExit(Collision other)
    //{
    //    if (other.collider.GetType() == typeof(SphereCollider))
    //    {
    //        Player.GetComponent<Sheepdog>().barkReactors.Remove(this);
    //    }
    //}
}
