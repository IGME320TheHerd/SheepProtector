using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float sprint = 20f;
    [SerializeField] Transform cam;

    [SerializeField] private Image staminaCircle; // ui component for stamina bar 
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float usageRate = 15f;
    [SerializeField] private float regenRate = 10f;

    Rigidbody rb;
    Vector2 moveInput = Vector2.zero;
    float currentSpeed;
    float currentStamina; //gets current stamina

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);

        Vector3 pos = transform.position;

        pos += moveInput.y * currentSpeed * Time.deltaTime * cam.forward;
        pos += moveInput.x * currentSpeed * Time.deltaTime * cam.right;

        rb.MovePosition(pos);
    }

    private void HandleStamina(bool isSprinting)
    {
        if (isSprinting) // if player running
        {
            currentStamina -= usageRate * Time.deltaTime; // decrease stamina relative to time
        }
        else if (currentStamina < maxStamina) // if not running 
        {
            currentStamina += regenRate * Time.deltaTime; // increase relative to time 
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // keeps stamina between 0 and maxStamina

        if (staminaCircle != null) // if theres ui circle assigned
        {
            staminaCircle.fillAmount = currentStamina / maxStamina; // update circle ui

            bool isFull = currentStamina >= maxStamina; // check for stamina full
            staminaCircle.enabled = !isFull; //if full get rid of ui
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        currentSpeed = moveSpeed;
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if(ctx.started && !ctx.performed)
        {
            currentSpeed = sprint;
            HandleStamina(true);
        }
        if (ctx.performed || ctx.canceled)
        {
            currentSpeed = moveSpeed;
        }
    }
}