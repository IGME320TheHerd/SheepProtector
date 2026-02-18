using System.Threading;
using UnityEngine;

public class Enemy : Animal
{
    // How long this enemy should be stunned for.
    private float barkStunTime;

    // The target this enemy is currently chasing, whether that be the sheep or sheepdog.
    private GameObject chaseTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    // How the enemy should react when the dog barks near them.
    protected override void BarkReaction(ContextCallback callBackContext)
    {
        
    }

    // Unused for enemies, as they are immediately destroyed when they die.
    protected override void Die()
    {
        // Unused for enemies, as they are immediately destroyed when they die.
    }

    // How the enemy should move.
    protected override void Movement()
    {
        
    }

    // Have the enemy start chasing a target.
    public void Chase(GameObject target)
    {

    }

    // How the enemy should attack the target.
    private void Attack()
    {

    }
}