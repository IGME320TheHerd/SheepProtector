using System.Threading;
using UnityEngine;

public class Sheepdog : Animal
{
    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void FixedUpdate()
    {
        
    }

    /// <summary>
    /// Required for compiling purposes, since the dog is the one barking, the dog does not need to react to it.
    /// </summary>
    /// <param name="callBackContext"></param>
    protected override void BarkReaction(ContextCallback callBackContext)
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
    private void Bark(ContextCallback callBackContext)
    {

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
