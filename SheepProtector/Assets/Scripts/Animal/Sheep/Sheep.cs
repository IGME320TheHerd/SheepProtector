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

    // The thirst meter for the sheep.
    private float thirst;

    // Fields for sheep movement.
    [SerializeField] private float maxSpeed = 0.1f;
    [SerializeField] private Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);

    // When fleeing, this is the target the sheep flees from (for right now, it should be the player).
    [SerializeField] private GameObject fleeTarget;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        // Sets the velocity of the sheep and the sheep's state to flee for now (will be removed in the future).
        velocity = new Vector3(100.0f, 0.0f, 100.0f);
        currentState = SheepState.Flee;
        ToFleeState(fleeTarget.transform.position);

        // Set the thirst level of the sheep to 0.
        thirst = 0.0f;
    }

    // Update is called once per frame
    protected override void Update()
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

        Movement();
    }

    // How the sheep should react when the dog barks.
    protected override void BarkReaction(ContextCallback callBackContext)
    {
        //ToFleeState(new Vector3(0.0f, 0.0f, 0.0f));
    }

    // What should happen if the sheep were to die.
    protected override void Die()
    {
        
    }

    // How the sheep should move depending on its current state.
    protected override void Movement()
    {
        switch(currentState)
        {
            case SheepState.Still:
                break;
            case SheepState.Wander:
                break;
            case SheepState.Flee:
                // Create the new direction the sheep should head in.
                float directionX = 0.0f;
                float directionZ = 0.0f;

                // Move the sheep away from the target in the x direction.
                directionX = -transform.position.x + fleeTarget.transform.position.x;

                // Move the sheep away from the target in the z direction.
                directionZ = -transform.position.z + fleeTarget.transform.position.z;

                // Update the sheep's fleeing direction based on the target's position.
                // (the sheep's fleeing direction will not be updating here in the future)
                direction = new Vector3(directionX, 0.0f, directionZ);
                direction.Normalize();

                // Update the position of the sheep based on the velocity and direction of the flee target.
                transform.position = new Vector3(transform.position.x - (velocity.x * direction.x),
                    transform.position.y,
                    transform.position.z - (velocity.z * direction.z));
                break;
        }
    }

    // How the sheep should transition everything to the flee state.
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

    // How the sheep should transition everything to the wander state.
    private void ToWanderState()
    {
        currentState = SheepState.Wander;
    }

    // How the sheep should transition everything to the still state.
    private void ToStillState()
    {
        currentState = SheepState.Still;
    }

    // Nudge the sheep in a desired direction.
    private void Nudge()
    {

    }

    // How the sheep should react upon a collision.
    private void OnCollisionEnter(Collision other)
    {
        
    }
}
