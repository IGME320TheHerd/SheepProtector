using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float moveSpeed = 10f; // movement speed
    public Transform camTransform; // reference to cameras positon

    private Rigidbody rb; // reference for player 
    private Vector3 movementDirection; // reference to store movement direction

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // attaches rigid body
    }

    void Update()
    {
        // uses WASD for movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 forward = camTransform.forward;  // gets forward direction pointing out from the camera
        Vector3 right = camTransform.right; // right direction from camera

        forward.y = 0; // sets vertivcal direction so players doesnt fly
        right.y = 0; // sets vert dir to keep movement on x and z
        
        // normalize movement to keep constant
        forward.Normalize(); 
        right.Normalize();

        movementDirection = (forward * v) + (right * h); // compare input and camera to get direction
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(movementDirection.x * moveSpeed, rb.linearVelocity.y, movementDirection.z * moveSpeed); // set physics velocity
        
        if (movementDirection != Vector3.zero) // check for player movement
        {
            transform.forward = Vector3.Slerp(transform.forward, movementDirection, Time.deltaTime * 10f); // rotates playe to face direction they are moving
        }
    }
}
