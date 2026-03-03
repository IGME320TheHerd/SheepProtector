using NUnit.Framework;
using System.Threading;
using UnityEngine;

public class Sheepdog : Animal
{
    // A list of all of the different animals that should react to bark.
    public System.Collections.Generic.List<Animal> barkReactors;

    // Checks if the bark button is held down.
    bool barkHeldDown;

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    private void Start()
    {
        barkHeldDown = false;
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
    public override void BarkReaction()//(ContextCallback callBackContext)
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
    public void Bark()//(ContextCallback callBackContext)
    {
        foreach (Animal animal in barkReactors)
        {
            animal.BarkReaction();
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
}
