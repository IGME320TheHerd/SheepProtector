using System.Threading;
using UnityEngine;

public class Enemy : Animal
{
    // How long this enemy should be stunned for.
    private float barkStunTime;
    private float stunTimer;
    private float stunCooldownTime;

    // The target this enemy is currently chasing, whether that be the sheep or sheepdog.
    private GameObject chaseTarget;
    private bool chasing;

    // Have the enemy wait to start chasing
    private bool readyStart;
    private float startTimer;
    private float maxStartTime;

    // Have the enemy wait to end chasing.
    private bool readyEnd;
    private float endTimer;
    private float maxEndTime;

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    private void Start()
    {
        chasing = false;
        stunTimer = -999.0f;

        // Set the bark stun time and cooldown time based on what type the enemy is.
        barkStunTime = 5.0f;
        stunCooldownTime = 3.0f;

        // Set the timers for both starting a chase and ending a chase based on what type the enemy is.
        maxStartTime = 5.0f;
        maxEndTime = 5.0f;
        startTimer = 0.0f;
        endTimer = 0.0f;

        readyStart = false;
        readyEnd = false;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Have the enemy move.
        acceleration = Vector3.zero;
        Movement();

        // Make sure that the enemy is not moving upwards.
        acceleration.y = 0.0f;
    }

    /// <summary>
    /// How the enemy should react when the dog barks near them.
    /// </summary>
    public override void BarkReaction()
    {
        if (stunTimer <= -1 * stunCooldownTime)
        {
            stunTimer = barkStunTime;
        }
    }

    /// <summary>
    /// Unused for enemies, as they are immediately destroyed when they die.
    /// </summary>
    public override void Die()
    {
        // Unused for enemies, as they are immediately destroyed when they die.
    }

    /// <summary>
    /// How the enemy should move.
    /// </summary>
    protected override void Movement()
    {
        // If the enemy is stunned from a bark, don't have it move.
        if (stunTimer > 0.0f)
        {
            stunTimer -= Time.deltaTime;
        }

        // If the enemy is starting to get ready to chase, have the timer go down until the chase starts.
         if (readyStart)
        {
            startTimer -= Time.deltaTime;

            // If the timer reaches 0, have the enemy start chasing.
            if (startTimer <= 0.0f)
            {
                readyStart = false;
                chasing = true;
            }
        }

        // If the enemy is starting to get ready to chase, have the timer go down until the chase starts.
        else if (readyEnd)
        {
            endTimer -= Time.deltaTime;

            // If the timer reaches 0, have the enemy stop chasing.
            if (endTimer <= 0.0f)
            {
                readyEnd = false;
                chasing = false;
                chaseTarget = null;
            }
        }

        // If the enemy is chasing something and is not stunned, have it move towards the chase target.
        if (chasing && stunTimer <= 0.0f)
        {
            if (stunTimer >= -1 * stunCooldownTime)
            {
                stunTimer -= Time.deltaTime;
            }

            float targetDist = Vector3.Distance(transform.position, chaseTarget.transform.position);

            acceleration += Seek(chaseTarget.transform.position);
            acceleration = Vector3.ClampMagnitude(acceleration, maxSpeed);
        }
        else
        {
            velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Have the enemy start chasing a target.
    /// </summary>
    /// <param name="target"> What this enemy should chase after, usually the sheepdog or the sheep. </param>
    public void Chase(GameObject target)
    {
        chasing = true;
        target = chaseTarget;
    }

    /// <summary>
    /// How the enemy should attack the target.
    /// </summary>
    private void Attack(GameObject target)
    {
        if (target.TryGetComponent<Animal>(out Animal deathTarget))
        {
            deathTarget.Die();
            stunTimer = barkStunTime;
        }
    }

    /// <summary>
    /// How the enemy should react upon a collision.
    /// </summary>
    /// <param name="other"> The other game object in the collision. </param>
    private void OnCollisionEnter(Collision other)
    {
        // If the collision is the sheep or sheepdog, have the enemy attack it.
        if ((other.gameObject.TryGetComponent<Sheepdog>(out Sheepdog otherDog)
            || other.gameObject.TryGetComponent<Sheep>(out Sheep otherSheep)) && stunTimer <= 0.0f)
        {
            // Attack the sheep or sheepdog in this collision.
            Attack(other.gameObject);
        }
    }

    /// <summary>
    /// How the enemy should react upon a collision.
    /// </summary>
    /// <param name="other"> The other game object in the collision. </param>
    private void OnCollisionStay(Collision other)
    {
        // If the collision is the sheep or sheepdog, have the enemy attack it.
        if ((other.gameObject.TryGetComponent<Sheepdog>(out Sheepdog otherDog)
            || other.gameObject.TryGetComponent<Sheep>(out Sheep otherSheep)) && stunTimer <= 0.0f)
        {
            // Attack the sheep or sheepdog in this collision.
            Attack(other.gameObject);
        }
    }

    /// <summary>
    /// How the enemy should react upon an enter trigger going off.
    /// </summary>
    /// <param name="other"> The other game object's collider. </param>
    public void OnTriggerEnter(Collider other)
    {
        // If the trigger is the sheep or sheepdog and the enemy is not already chasing,
        // have the enemy get ready to chase it.
        if ((other.gameObject.TryGetComponent<Sheepdog>(out Sheepdog otherDog) && other.GetType() != typeof(SphereCollider))
            || other.gameObject.TryGetComponent<Sheep>(out Sheep otherSheep))
        {
            // If the enemy caught up to the thing it is chasing, remove the end timer and have the enemy continue chasing.
            if (other.gameObject == chaseTarget && readyEnd)
            {
                endTimer = 0.0f;
                readyEnd = false;
            }

            // If the enemy is not already chasing a target, have it target the other gameobject.
            else if (chaseTarget == null)
            {
                chaseTarget = other.gameObject;
                startTimer = maxStartTime;
                readyStart = true;
            }
        }
    }

    /// <summary>
    /// How the enemy should react upon hitting an exit trigger going off.
    /// </summary>
    /// <param name="other"> The other game object's collider. </param>
    public void OnTriggerExit(Collider other)
    {
        // If the trigger is the sheep or sheepdog and the enemy is chasing them,
        // have the enemy get ready to stop chasing it.
        if (other.gameObject == chaseTarget && other.GetType() != typeof(SphereCollider))
        {
            // If the enemy has not started chasing yet, reset the start timer stuff and there is no need for the end timer.
            if (readyStart)
            {
                chaseTarget = null;
                startTimer = 0.0f;
                readyStart = false;
            }

            // If the enemy is chasing a target, give it a few seconds to end the chase, incase the enemy catches up to the prey.
            else
            {
                // Have the enemy stop chasing after a few seconds.
                readyEnd = true;
                endTimer = maxEndTime;
            }
        }
    }

    protected override Vector3 CalcSteering()
    {
        return Vector3.zero;
        //throw new System.NotImplementedException();
    }
}