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
    bool barkHeldDown;

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    private void Start()
    {
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
    }

    /// <summary>
    /// Required for compiling purposes, since the dog is the one barking, the dog does not need to react to it.
    /// </summary>
    /// <param name="callBackContext"></param>
    public override void BarkReaction()
    {
        // Required for compiling purposes, since the dog is the one barking, the dog does not need to react to it.
    }

    /// <summary>
    /// What should happen if the sheepdog were to die.
    /// </summary>
    protected override void Die()
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
    public void Bark()
    {
        for (int i = 0; i < barkReactors.Count; i++)
        {
            barkReactors[i].BarkReaction();
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
        // If the trigger is an animalk, add if to the dog's list of bark reactions if it is not already there.
        if (other.gameObject.TryGetComponent<Animal>(out Animal otherAnimal))
        {
            if (!barkReactors.Contains(otherAnimal))
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
            if (barkReactors.Contains(otherAnimal))
            {
                barkReactors.Remove(otherAnimal);
            }

            if (other.gameObject.TryGetComponent<Sheep>(out Sheep otherSheep))
            {
                otherSheep.LeaveBark();
            }
        }
    }

    protected override Vector3 CalcSteering()
    {
        throw new System.NotImplementedException();
    }
}
