using System.Threading;
using UnityEngine;

public class Enemy : Animal
{
    // How long this enemy should be stunned for.
    private float barkStunTime;

    // The target this enemy is currently chasing, whether that be the sheep or sheepdog.
    private GameObject chaseTarget;

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
    /// How the enemy should react when the dog barks near them.
    /// </summary>
    /// <param name="callBackContext"></param>
    protected override void BarkReaction(ContextCallback callBackContext)
    {
        
    }

    /// <summary>
    /// Unused for enemies, as they are immediately destroyed when they die.
    /// </summary>
    protected override void Die()
    {
        // Unused for enemies, as they are immediately destroyed when they die.
    }

    /// <summary>
    /// How the enemy should move.
    /// </summary>
    protected override void Movement()
    {
        
    }

    /// <summary>
    /// Have the enemy start chasing a target.
    /// </summary>
    /// <param name="target"> What this enemy should chase after, usually the sheepdog or the sheep. </param>
    public void Chase(GameObject target)
    {

    }

    /// <summary>
    /// How the enemy should attack the target.
    /// </summary>
    private void Attack()
    {

    }
}