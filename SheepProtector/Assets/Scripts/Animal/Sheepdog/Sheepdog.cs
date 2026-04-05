using NUnit.Framework;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sheepdog : Animal
{
    // A list of all of the different animals that should react to bark.
    private System.Collections.Generic.List<Animal> barkReactors;

    /// <summary>
    /// Making barkReactors public so other things can add themselves to it.
    /// </summary>
    public System.Collections.Generic.List<Animal> BarkReactors
    {
        get { return barkReactors; }
        set { barkReactors = value; }
    }

    // Checks if the bark button is held down.
    private bool barkHeldDown;
    [SerializeField] private float maxBarkCooldown = 0.5f;
    private float barkCooldownTimer = 0.0f;

    // The cooldown for herding.
    [SerializeField] private float herdDist = 5.0f;
    [SerializeField] private float herdStrength = 500;

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    private void Start()
    {
        // Set up barking.
        barkHeldDown = false;
        barkReactors = new System.Collections.Generic.List<Animal>();
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void FixedUpdate()
    {
        // Decrease the bark cooldown.       
        if (barkCooldownTimer > -99.0f)
        {
            barkCooldownTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Required for compiling purposes, since the dog is the one barking, the dog does not need to react to it.
    /// </summary>
    public override void BarkReaction()
    {
        // Required for compiling purposes, since the dog is the one barking, the dog does not need to react to it.
    }

    /// <summary>
    /// What should happen if the sheepdog were to die.
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

    /// <summary>
    /// Not used since the controls are elsewhere.
    /// </summary>
    protected override void Movement()
    {
        // Not used since the controls are elsewhere.
    }

    /// <summary>
    /// What should happen when the player uses the bark button.
    /// </summary>
    /// <param name="context"> The input button pressed to call this function. </param>
    public void Bark(InputAction.CallbackContext context)
    {
        // When the player presses down the bark button, make sure bark is off cooldown before activating again.
        if (!barkHeldDown && barkCooldownTimer <= 0.0f)
        {
            barkHeldDown = true;
            barkCooldownTimer = maxBarkCooldown;

            // Have all of the bark reactions go off.
            for (int i = 0; i < barkReactors.Count; i++)
            {
                if (barkReactors[i] != null)
                {
                    barkReactors[i].BarkReaction();

                    // If the bark reactor is the sheep, tell the sheep that
                    // it no longer needs to check if the sheepdog is too close unless the sheep is no longer fleeing from the sheepdog.
                    if (barkReactors[i].gameObject.TryGetComponent<Sheep>(out Sheep sheep))
                    {
                        sheep.TooClose = false;
                        //sheep.InRangeBarkCheck = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// How the sheepdog should pick up objects lying around the map.
    /// </summary>
    private void PickUp()
    {

    }

    /// <summary>
    /// How the sheepdog should react upon a collision.
    /// </summary>
    /// <param name="other"> The other game object in the collision. </param>
    //private void OnCollisionEnter(Collision other)
    //{
    //
    //}

    /// <summary>
    /// How the sheepdog should react upon an enter trigger going off.
    /// </summary>
    /// <param name="other"> The other game object's collider. </param>
    public void OnTriggerEnter(Collider other)
    {
        // If the trigger is an animal, add if to the dog's list of bark reactions if it is not already there.
        if (other.gameObject.TryGetComponent<Animal>(out Animal otherAnimal)) 
        {
            if (!barkReactors.Contains(otherAnimal) && !other.isTrigger)
            {
                barkReactors.Add(otherAnimal);
            }
        }
    }

    /// <summary>
    /// How the sheepdog should react upon hitting an exit trigger going off.
    /// </summary>
    /// <param name="other"> The other game object's collider. </param>
    public void OnTriggerExit(Collider other)
    {
        // If the other object is an animal, remove it from the dog's list of bark reactions
        if (other.gameObject.TryGetComponent<Animal>(out Animal otherAnimal))
        {
            // Check to see if the other is the sheep and if it is the sheep, check to see if it is the sheep's sphere trigger.
            bool sheep = other.gameObject.TryGetComponent<Sheep>(out Sheep otherSheep);

            // Remove the other animal to from the list of bark reactors if it is not the sheep.
            if (!sheep && barkReactors.Contains(otherAnimal) && !other.isTrigger)
            {
                barkReactors.Remove(otherAnimal);
            }

            // If the other animal is the sheep, remove it from barkReactors if it is not the sheep's sphere trigger.
            else if (sheep && !other.isTrigger)
            {
                barkReactors.Remove(otherAnimal);
                otherSheep.LeaveBark();
            }
        }
    }

    protected override Vector3 CalcSteering()
    {
        throw new System.NotImplementedException();
    }
}
