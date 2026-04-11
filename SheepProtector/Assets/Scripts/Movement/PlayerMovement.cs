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

    private Rigidbody rb; // reference for player 
    private Vector3 movementDirection; // reference to store movement direction
    private float currentStamina; //gets current stamina
    private float currentSpeed; // tracks current speed
    float h;
    float v;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // attaches rigid body
        currentStamina = maxStamina; // start with max stamina
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            h = ctx.ReadValue<Vector2>().x;
            v = ctx.ReadValue<Vector2>().y;
            currentSpeed = moveSpeed;
            Debug.Log(movementDirection);
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
            staminaCircle.fillAmount = currentStamina / maxStamina; // update circle ui

            bool isFull = currentStamina >= maxStamina; // check for stamina full
            staminaCircle.enabled = !isFull; //if full get rid of ui
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(movementDirection.x * currentSpeed, 0, movementDirection.y * currentSpeed); // set physics velocity
    }
}
