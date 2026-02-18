using System.Threading;
using UnityEngine;

public class Sheepdog : Animal
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    // Required for compiling purposes, since the dog is the one barking, the dog does not need to react to it.
    protected override void BarkReaction(ContextCallback callBackContext)
    {
        // Required for compiling purposes, since the dog is the one barking, the dog does not need to react to it.
    }

    // What should happen if the sheepdog were to die.
    protected override void Die()
    {
        
    }

    // How the sheepdog should move.
    protected override void Movement()
    {
        
    }
    
    // What should happen when the player uses the bark button.
    private void Bark(ContextCallback callBackContext)
    {

    }

    // How the sheepdog should pick up objects lying around the map.
    private void PickUp()
    {

    }

    // How the sheepdog should react upon a collision.
    private void OnCollisionEnter(Collision other)
    {

    }
}
