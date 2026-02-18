using System.Threading;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    // The variables that make up each animal.
    protected SpriteRenderer spriteRenderer;
    protected float speed;
    protected bool isAlive = true;

    // Before the first execution of Update, set up all variables and anything else that needs setting up.
    protected abstract void Start();

    // Called once a frame, update anything that needs to be updated and check anything that needs to be checked.
    protected abstract void Update();

    // How each animal should react (if at all) when the sheepdog barks.
    protected abstract void BarkReaction(ContextCallback callBackContext);

    // How the animal should move.
    protected abstract void Movement();

    // How the animal should die (if at all).
    protected abstract void Die();
}
