using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rBody;

    [SerializeField]
    protected float maxSpeed;

    protected Vector3 velocity, acceleration, steeringForce, wanderTarget;

    private Quaternion nextRotation;

    public Vector3 Velocity { get { return velocity; } }

    // Start is called before the first frame update
    void Start()
    {
        nextRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        acceleration = Vector3.zero;
        steeringForce = CalcSteering();
        acceleration += steeringForce;
    }

    private void FixedUpdate()
    {
        if (acceleration.magnitude <= 0)
        {
            acceleration = -velocity * 2.0f;
        }

        Vector3 nextPos = transform.position;

        velocity += acceleration * Time.fixedDeltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        velocity = new Vector3(velocity.x, 0.0f, velocity.z);

        if (velocity.magnitude != 0)
        {
           nextRotation = Quaternion.LookRotation(velocity, Vector3.up);
           nextPos += (velocity * Time.fixedDeltaTime);

            rBody.Move(nextPos, nextRotation);
        }
        acceleration = Vector3.zero;
    }

    protected abstract Vector3 CalcSteering();

    protected Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desiredVel = targetPos - transform.position;
        desiredVel = desiredVel.normalized * maxSpeed;

        return desiredVel - velocity;
    }

    protected Vector3 Seek(GameObject target)
    {
        return Seek(target.transform.position);
    }

    protected Vector3 Arrive(Vector3 targetPos, float radius)
    {
        Vector3 desiredVel = targetPos - transform.position;

        float distance = desiredVel.magnitude;

        if (distance < radius)
        {
            desiredVel = desiredVel.normalized * (maxSpeed * distance / radius);
        }
        else
        {
            desiredVel = desiredVel.normalized * maxSpeed;
        }

        Debug.Log(desiredVel.magnitude);
        return desiredVel - velocity;
    }

    protected Vector3 Arrive(GameObject target, float radius)
    {
        return Arrive(target.transform.position, radius);
    }

    protected Vector3 Flee(Vector3 fleeTarget)
    {
        Vector3 desiredVel = transform.position - fleeTarget;
        desiredVel = desiredVel.normalized * maxSpeed;
        return desiredVel - velocity;
    }

    protected Vector3 Flee(GameObject target)
    {
        return Flee(target.transform.position);
    }

    protected Vector3 Separate(Vector3[] separateSet, float sepDistance)
    {
        if (separateSet.Length == 0)
        {
            return Vector3.zero;
        }

        Vector3 seperateForce = Vector3.zero;

        foreach (Vector3 loc in separateSet)
        {
            Vector3 fleeForce = Flee(loc);
            float distance = Vector3.Distance(loc, transform.position);

            fleeForce *= 1.0f - (distance / sepDistance);
            seperateForce += fleeForce;
        }

        return seperateForce.normalized * maxSpeed;
    }

    protected Vector3 Alignment(Vector3 direction)
    {
        return (direction.normalized * maxSpeed) - velocity;
    }

    protected Vector3 Wander(float radius, float distance, float jitter)
    {
        if (wanderTarget == Vector3.zero)
        {
            wanderTarget = Random.onUnitSphere * radius;
        }

        wanderTarget += new Vector3(
            Random.Range(-1.0f, 1.0f) * jitter,
            Random.Range(-1.0f, 1.0f) * jitter,
            Random.Range(-1.0f, 1.0f) * jitter);

        wanderTarget = wanderTarget.normalized * radius;

        Vector3 targetInWorldSpace = transform.position + (velocity.normalized * distance) + wanderTarget;

        return Seek(targetInWorldSpace);
    }

    protected Vector3 Pursue(Agent pursueTarget)
    {
        Vector3 targetPos = pursueTarget.transform.position;
        Vector3 prediction = pursueTarget.velocity;
        prediction = prediction * 5f;
        targetPos += prediction;
        return Seek(targetPos);
    }

    protected Vector3 Evade(Agent evadeTarget)
    {
        Vector3 targetPos = evadeTarget.transform.position;
        Vector3 prediction = evadeTarget.velocity;
        prediction = prediction * 5f;
        targetPos += prediction;
        return Flee(targetPos);
    }

    protected Vector3 Evade(Rigidbody evadeTarget)
    {
        Vector3 targetPos = evadeTarget.transform.position;
        Vector3 prediction = evadeTarget.linearVelocity;
        prediction = prediction * 2f;
        targetPos += prediction;
        return Flee(targetPos);
    }
}
