using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; // movement speed
    [SerializeField] private float sprint = 15f; // sprint speed
    [SerializeField] private Transform camTransform; // reference to cameras positon

    [SerializeField] private Image staminaCircle; // ui component for stamina bar 
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float usageRate = 15f;
    [SerializeField] private float regenRate = 10f;
    [SerializeField] private Gradient staminaGradient;

    [SerializeField] private GameObject barkSprite;

    private Rigidbody rb; // reference for player 
    private Animator animator;
    private Vector3 movementDirection; // reference to store movement direction
    private float currentStamina; //gets current stamina
    private float currentSpeed; // tracks current speed
    private SpriteRenderer sr;
    private Vector3 currentDir;
    float h;
    float v;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // attaches rigid body
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        currentStamina = maxStamina; // start with max stamina
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            h = ctx.ReadValue<Vector2>().x;
            v = ctx.ReadValue<Vector2>().y;
            currentSpeed = moveSpeed;
        }

        if (ctx.canceled)
        {
            h = 0.0f;
            v = 0.0f;
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("Sprinting!");
            currentSpeed = sprint;
        }
    }

    void Update()
    {
        // uses WASD for movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 camForward = new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z);
        float angle = Vector3.SignedAngle(currentDir, camForward, Vector3.up);

        if (angle > 5 && angle < 175)
        {
            sr.flipX = true;
            barkSprite.transform.localPosition = new Vector3(-8.22f, 3.32f, 0.0f);
            barkSprite.GetComponent<SpriteRenderer>().flipX = true;

            if(staminaCircle != null)
            {
                RectTransform rect = staminaCircle.rectTransform;
                rect.localScale = new Vector3(-1, 1, 1);

                Vector3 pos = rect.localPosition;
                pos.x = -Mathf.Abs(pos.x);
                rect.localPosition = pos;
            }
        }
        else if (angle < -5 && angle > -175)
        {
            sr.flipX = false;
            barkSprite.transform.localPosition = new Vector3(8.22f, 3.32f, 0.0f);
            barkSprite.GetComponent<SpriteRenderer>().flipX = false;

            if (staminaCircle != null)
            {
                RectTransform rect = staminaCircle.rectTransform;
                rect.localScale = new Vector3(1, 1, 1);

                Vector3 pos = rect.localPosition;
                pos.x = Mathf.Abs(pos.x);
                rect.localPosition = pos;
            }
        }

        bool isMoving = h != 0 || v != 0; // see if theres movement based on horzintal and vertical movement
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && isMoving && currentStamina > 0; // if shift is down and you have stamina

        if (isSprinting) // if sprinting
        {
            currentSpeed = sprint; // use sprint speed
        }
        else
        {
            currentSpeed = moveSpeed; // else use reg speed
        }

        Vector3 forward = camTransform.forward;  // gets forward direction pointing out from the camera
        Vector3 right = camTransform.right; // right direction from camera

        forward.y = 0; // sets vertivcal direction so players doesnt fly
        right.y = 0; // sets vert dir to keep movement on x and z

        // normalize movement to keep constant
        forward.Normalize();
        right.Normalize();


        movementDirection = (forward * v) + (right * h); // compare input and camera to get direction

        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("walkSpeed", rb.linearVelocity.magnitude/moveSpeed);

        if (isMoving)
        {
            currentDir = movementDirection;
            
        }

        HandleStamina(isSprinting); // use stamina function
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

        if(staminaCircle != null) // if theres ui circle assigned
        {
            float fillRatio = currentStamina / maxStamina;
            staminaCircle.fillAmount = fillRatio; // update circle ui

            staminaCircle.color = staminaGradient.Evaluate(fillRatio);

            bool isFull = currentStamina >= maxStamina; // check for stamina full
            staminaCircle.enabled = !isFull; //if full get rid of ui
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(movementDirection.x * currentSpeed, rb.linearVelocity.y, movementDirection.z * currentSpeed); // set physics velocity
    }
}
