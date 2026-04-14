using Unity.VisualScripting;
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
    [SerializeField] private GameObject barkSprite;

    Rigidbody rb;
    Vector2 moveInput = Vector2.zero;
    float currentSpeed;
    float currentStamina; //gets current stamina
    Vector3 currentDir = Vector3.zero;
    SpriteRenderer playerSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerSprite = GetComponentInChildren<SpriteRenderer>(); 
    }

    private void FixedUpdate()
    {
        Vector3 camForward = new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z);
        float angle = Vector3.SignedAngle(currentDir, camForward, Vector3.up);

        if (angle > 5 && angle < 175)
        {
            playerSprite.flipX = true;
            barkSprite.transform.localPosition = new Vector3(-5.5f, 1.83f, 0.0f);
            barkSprite.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (angle < -5 && angle > -175)
        {
            playerSprite.flipX = false;
            barkSprite.transform.localPosition = new Vector3(5.5f, 1.83f, 0.0f);
            barkSprite.GetComponent<SpriteRenderer>().flipX = false;
        }

        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);

        Vector3 pos = transform.position;

        pos += moveInput.y * currentSpeed * Time.deltaTime * cam.forward;
        pos += moveInput.x * currentSpeed * Time.deltaTime * cam.right;

        currentDir = pos;
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