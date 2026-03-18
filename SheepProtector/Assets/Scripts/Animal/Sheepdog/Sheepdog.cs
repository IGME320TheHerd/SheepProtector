using NUnit.Framework;
using System.Threading;
using UnityEngine;

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

    // The cooldown for herding.
    private bool herdPressed;
    private float herdCooldown;
    private float maxHerdCooldown;
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

        // Set up herding.
        herdPressed = false;
        herdCooldown = 0.0f;
        maxHerdCooldown = 3.0f;
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void FixedUpdate()
    {
        // Check to see if the player wants to bark.
        bool barkCheck = Input.GetButton("Bark");

        // When the player presses down the bark button, have all bark actions go off.
        if (barkCheck && !barkHeldDown)
        {
            Bark();
            barkHeldDown = true;
        }

        // If the player is no longer holding down the bark button, reset the held down checker.
        else if (!barkCheck && barkHeldDown)
        {
            barkHeldDown = false;
        }

        // If the sheepdog has recently herded, decrease the cooldown so it can herd again.
        if (herdPressed)
        {
            herdCooldown -= Time.deltaTime;
            
            // If the herd cooldown is over, allow the sheepdog to herd again.
            if (herdCooldown <= 0.0f)
            {
                herdPressed = false;
            }
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
    /// <param name="callBackContext"></param>
    private void Bark()
    {
        for (int i = 0; i < barkReactors.Count; i++)
        {
            barkReactors[i].BarkReaction();

            // If the bark reactor is the sheep, tell the sheep that
            // it no longer needs to check if the sheepdog is too close unless the sheep is no longer fleeing from the sheepdog.
            if (barkReactors[i].gameObject.TryGetComponent<Sheep>(out Sheep sheep))
            {
                sheep.TooClose = false;
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
    private void OnCollisionEnter(Collision other)
    {

    }

    /// <summary>
    /// How the sheepdog should react upon an enter trigger going off.
    /// </summary>
    /// <param name="other"> The other game object's collider. </param>
    public void OnTriggerEnter(Collider other)
    {
        // If the trigger is an animal, add if to the dog's list of bark reactions if it is not already there.
        if (other.gameObject.TryGetComponent<Animal>(out Animal otherAnimal)) 
        {
            bool sheepClose = other.gameObject.TryGetComponent<Sheep>(out Sheep otherSheep) 
                && other.GetType() != typeof(SphereCollider);
            if (!barkReactors.Contains(otherAnimal) && !sheepClose)
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
            bool sheepSphere = other.GetType() == typeof(SphereCollider);

            // Remove the other animal to from the list of bark reactors if it is not the sheep.
            if (!sheep && barkReactors.Contains(otherAnimal))
            {
                barkReactors.Remove(otherAnimal);
            }

            // If the other animal is the sheep, remove it from barkReactors if it is not the sheep's sphere trigger.
            else if (sheep && !sheepSphere)
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
