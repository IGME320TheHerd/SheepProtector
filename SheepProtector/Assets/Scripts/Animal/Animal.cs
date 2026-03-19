using System.Threading;
using UnityEngine;

public abstract class Animal : Agent
{
    // The variables that make up each animal.
    protected SpriteRenderer spriteRenderer;
    protected float speed;
    protected bool isAlive = true;

    /// <summary>
    /// How each animal should react (if at all) when the sheepdog barks.
    /// </summary>
    public abstract void BarkReaction();

    /// <summary>
    /// How the animal should move.
    /// </summary>
    protected abstract void Movement();

    /// <summary>
    /// How the animal should die (if at all).
    /// </summary>
    public abstract void Die();
}
