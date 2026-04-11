using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The different types of enemies.
/// </summary>
enum EnemyType
{
    Wolf,
    Bear
}
public class Enemy : Animal
{
    // How long this enemy should be stunned for. Only used for the wolf.
    private float barkStunTime;
    private float stunTimer;
    private float stunCooldownTime;

    // The timer for how long the enemy should change their target to the player for. Only used for the bear.
    private float chasePlayerTimer;
    private float maxChasePlayerTime;
    private float slowDist;

    // The target this enemy is currently chasing, which should be the sheep.
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

    // Fields for controlling the enemy's speed.
    private float topSpeed;
    private float slowedSpeed;
    private float slowedTimer;
    private float slowCooldownTime;
    private float barkSlowTime;

    // If the enemy is wandering a designated area or not.
    private bool wandering;
    private float wanderTimer;
    private float maxWanderTime;
    private float wanderSpeed;
    private Vector3 wanderPos;

    // The starting position of the enemy.
    private Vector3 home;
    private float maxHomeDist; // How far the enemy can be from its home before it stops chasing.
    private bool goHome = false;
    private NavMeshAgent agent;

    // What type the enemy is.
    [SerializeField] private EnemyType type;

    // The player gameobject.
    [SerializeField] private GameObject player;

    // If the chaseTarget is behind a wall and the enemy does not have line of sight, have that enemy go to the last known position.
    private Vector3 lastPosition;
    private bool startFinding;

    // The layer mask of the sheepdog.
    public LayerMask dogMask;

    // The layer mask of all walls.
    public LayerMask wallMask;

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    private void Start()
    {
        chasing = false;
        stunTimer = -999.0f;

        home = transform.position;
        agent = GetComponent<NavMeshAgent>();

        // Set all of the specific variables that change based on what type the enemy is.
        switch (type)
        {
            // Set all of the specific wolf variables.
            case EnemyType.Wolf:
                maxStartTime = 2.0f;
                maxEndTime = 5.0f;
                topSpeed = 10.0f;
                slowedSpeed = 5.0f;
                slowedTimer = 5.0f;
                barkStunTime = 5.0f;
                barkSlowTime = 5.0f;
                stunCooldownTime = 3.0f;
                slowCooldownTime = 3.0f;
                wanderSpeed = 6.0f;
                maxHomeDist = 60.0f;
                break;

            // Set all of the specific bear variables.
            case EnemyType.Bear:
                maxStartTime = 1.0f;
                maxEndTime = 3.0f;
                topSpeed = 8.0f;
                slowedSpeed = 4.0f;
                barkSlowTime = 2.0f;
                slowCooldownTime = 2.0f;
                maxChasePlayerTime = 1.3f;
                slowDist = 5.0f;
                wanderSpeed = 5.0f;
                maxHomeDist = 55.0f;
                break;
        }

        startTimer = 0.0f;
        endTimer = 0.0f;
        readyStart = false;
        readyEnd = false;
        stunTimer = -99.0f;
        startFinding = false;
        agent.speed = topSpeed;
        maxWanderTime = 5.0f;

        // If the enemy's reference to the player game object is null, grab it.
        if (player == null)
        {
            player = FindAnyObjectByType<Sheepdog>().gameObject;
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Have the enemy move.
        Movement();

        // Check to see if the enemy has moved too far away from its home.
        if (chasing && Vector3.Distance(transform.position, home) > maxHomeDist)
        {
            // If the enemy has moved too far from its home, have it move back.
            goHome = true;
            agent.destination = home;
            agent.speed = topSpeed;
        }

        // If the enemy has made it back to its home, have it stop and go back to the still state of the enemy.
        else if (goHome && Vector3.Distance(transform.position, home) <= 7.0)
        {
            goHome = false;
            agent.speed = 0;
            agent.destination = transform.position;
        }
    }

    /// <summary>
    /// How the enemy should react when the dog barks near them.
    /// </summary>
    public override void BarkReaction()
    {
        // Have the enemy react in different ways depending on the enemy's type.
        switch (type)
        {
            case EnemyType.Wolf:
                // Set the positions of the player and the sheep in convienient variables.
                Vector3 sheepdogPos = player.transform.position;
                Vector3 sheepPos;

                // If the wolf is currently chasing the sheep, get the position of the chase target.
                if (chaseTarget != null)
                {
                    sheepPos = chaseTarget.transform.position;
                }

                // If the wolf is not currently chasing the sheep, get the sheep's position anyway.
                else
                {
                    sheepPos = FindAnyObjectByType<Sheep>().transform.position;
                }

                // Get the angle at which the sheepdog is away from the wolf.
                Vector3 sheepdogAngle = sheepdogPos - transform.position;
                sheepdogAngle = sheepdogAngle.normalized;

                // Get the angle at which the sheep is away from the wolf.
                Vector3 sheepAngle = sheepPos - transform.position;
                sheepAngle = sheepAngle.normalized;

                // Raycast to see if the sheepdog is between the wolf and sheep.
                RaycastHit hit;

                Debug.DrawRay(transform.position, sheepAngle, Color.blue, 10.0f);

                // Check to see if the sheepdog is in front of the wolf.
                if (Physics.SphereCast(transform.position, 10.0f, sheepAngle, out hit, 100.0f, dogMask, QueryTriggerInteraction.Ignore)
                    && Vector3.Distance(transform.position, sheepdogPos) < Vector3.Distance(transform.position, sheepPos))
                {
                    // Freeze the wolf in place if enough time has passed.
                    if (stunTimer <= -1 * stunCooldownTime)
                    {
                        stunTimer = barkStunTime;
                    }
                }

                // If the sheepdog is not in front of the wolf, have the wolf slow down temporarily due to being spooked.
                else
                {
                    // Slow down the wolf if enough time has passed.
                    if (slowedTimer <= -1 * slowCooldownTime)
                    {
                        agent.speed = slowedSpeed;
                        slowedTimer = barkSlowTime;
                    }
                }
                    break;

            case EnemyType.Bear:
                chasePlayerTimer = maxChasePlayerTime;

                // If the bear is close enough to the sheep, have it get slowed when the dog barks.
                if (Vector3.Distance(transform.position, chaseTarget.transform.position) > slowDist)
                {
                    slowedTimer = barkSlowTime;
                    agent.speed = slowedSpeed;
                }
                    break;
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
        if (stunTimer >= -1 * stunCooldownTime)
        {
            stunTimer -= Time.deltaTime;
        }

        // Need a default raycast for hitFound for compiling, hitFound will not be used unless wall casting has actually been checked.
        RaycastHit hitFound;
        Physics.Raycast(transform.position, Vector3.zero, out hitFound);
        bool wallCast;
        Vector3 sheepAngle = Vector3.zero;

        // If a chase target has not yet been assigned, make wall cast true.
        if (chaseTarget == null)
        {
            wallCast = true;
        }
        else
        {
            sheepAngle = chaseTarget.transform.position - transform.position;
            sheepAngle = sheepAngle.normalized;

            wallCast = Physics.Raycast(transform.position, sheepAngle, out hitFound,
                    Vector3.Distance(transform.position, chaseTarget.transform.position),
                    wallMask);
        }

        // If the enemy is starting to get ready to chase, have the timer go down until the chase starts.
        if (readyStart && !wallCast)
        {
            startTimer -= Time.deltaTime;

            // If the timer reaches 0, have the enemy start chasing.
            if (startTimer <= 0.0f)
            {
                readyStart = false;
                chasing = true;
                agent.SetDestination(chaseTarget.transform.position);
            }
        }

        // If the enemy is getting ready to stop chasing, have the timer go down until the chase starts.
        else if (readyEnd)
        {
            endTimer -= Time.deltaTime;

            // If the timer reaches 0 or the enemy is on its way back to its home, have the enemy stop chasing.
            if (endTimer <= 0.0f || goHome)
            {
                goHome = true;
                readyEnd = false;
                chasing = false;
                chaseTarget = null;
                endTimer = -1.0f;
                agent.SetDestination(home);
            }
        }

        // If the enemy is slowed down, have the slowed timer decrease.
        if (slowedTimer > -1 * slowCooldownTime)
        {
            slowedTimer -= Time.deltaTime;
        }

        // If the enemy just stopped being slowed down, or frozen, reset the max speed to the enemy's top speed.
        else if (agent.speed == slowedSpeed || (agent.speed == 0.0f && stunTimer <= 0.0f && chasing))
        {
            agent.speed = topSpeed;
        }

        if (startFinding)
        {
            // If there are no walls blocking the enemy from the sheep, start chasing the sheep.
            if (!wallCast)
            {
                // If the enemy caught up to the thing it is chasing, remove the end timer and have the enemy continue chasing.
                if (readyEnd)
                {
                    endTimer = 0.0f;
                    readyEnd = false;
                }
                else
                {
                    startFinding = false;
                    startTimer = maxStartTime;
                    readyStart = true;
                    wandering = false;
                }
            }
        }

        // If the enemy is not chasing something, have it wander around its home area.
        else if (wandering && wallCast)
        {
            wanderTimer -= Time.deltaTime;

            // If the enemy wanders for too long, have it stop wandering.
            if (wanderTimer <= 0.0f || transform.position == wanderPos)
            {
                wandering = false;
                agent.speed = topSpeed;
                agent.SetDestination(transform.position);
                wanderPos = Vector3.zero;
                wanderTimer = maxWanderTime;
            }
        }

        // If the enemy is chasing something and is not stunned, have it move towards the chase target.
        else if (chasing && stunTimer <= 0.0f)
        {
            // If the enemy has changed target to the player, have that enemy chase the player.
            if (chasePlayerTimer > 0.0f)
            {
                chasePlayerTimer -= Time.deltaTime;
                agent.SetDestination(player.transform.position);
            }

            // If the enemy has not changed its target, have it chase the chase target.
            else
            {
                if (chaseTarget != null)
                {
                    Debug.DrawRay(transform.position, sheepAngle *
                        Vector3.Distance(transform.position, chaseTarget.transform.position) / 2, Color.red, 1.0f);
                }

                // If the enemy loses line of sight, have it travel towards the last known position.
                if (wallCast)
                {
                    if (!hitFound.collider.gameObject.TryGetComponent<Animal>(out Animal otherAnimal))
                    {
                        agent.SetDestination(lastPosition);

                        // If the enemy has reached the last known position and cannot see the chase target still, have it stop chasing.
                        if (transform.position == lastPosition)
                        {
                            chasing = false;
                            agent.SetDestination(transform.position);
                        }
                    }
                    else
                    {
                        lastPosition = chaseTarget.transform.position;
                        agent.SetDestination(chaseTarget.transform.position);
                    }
                }
                else
                {
                    lastPosition = chaseTarget.transform.position;
                    agent.SetDestination(chaseTarget.transform.position);
                }
                
            }
        }

        // If the enemy is stunned, don't have it move.
        else if (chasing || goHome)
        {
            // This is only here to prevent anything else from going off.
        }

        // If the enemy has lost sight, have it travel towards the last known position.
        // If the enemy has no target, have it wander around.
        else
        {
            if (chaseTarget != null)
            {
                Debug.DrawRay(transform.position, sheepAngle, Color.red, 1.0f);
                // If the enemy loses line of sight, have it travel towards the last known position.
                if (!wallCast)
                {
                    agent.SetDestination(lastPosition);

                    // If the enemy has reached the last known position and cannot see the chase target still, have it stop chasing.
                    if (transform.position == lastPosition)
                    {
                        chasing = false;
                        agent.SetDestination(transform.position);
                    }
                }
            }
            else
            {
                wanderTimer -= Time.deltaTime;
                agent.speed = 0.0f;

                // If the random wander timer reaches 0, check to see if the enemy should wander.
                if (wanderTimer <= 0.0f)
                {
                    // If the enemy passes a random number check, have it wander to a random nearby point.
                    float rng = Random.Range(0, 100);
                    if (rng <= 40.0f)
                    {
                        wandering = true;
                        wanderPos = Random.onUnitSphere * 7;
                        agent.SetDestination(wanderPos);
                        agent.speed = wanderSpeed;
                    }

                    wanderTimer = maxWanderTime;
                }
            }
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
        if ((other.gameObject.TryGetComponent<Sheep>(out Sheep otherSheep)
            || (other.gameObject.TryGetComponent<Sheepdog>(out Sheepdog otherDog) && type == EnemyType.Bear)) && stunTimer <= 0.0f)
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
        // If the trigger is the sheep, have the enemy get ready to chase it.
        if (other.gameObject.TryGetComponent<Sheep>(out Sheep otherSheep) && !other.isTrigger)
        {
            // Move the checks to movement because C# and Unity don't like raycasts in OnTriggerEnter.
            chaseTarget = other.gameObject;
            startFinding = true;
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
            startFinding = false;
            
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