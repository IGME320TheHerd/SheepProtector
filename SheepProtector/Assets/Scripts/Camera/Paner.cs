using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float panSpeed = 6f; // camera speed
    private Camera cam; // camera reference


    private void Awake()
    {
        cam = GetComponentInChildren<Camera>(); // gets camera component attached to empty object
    }

    // Update is called once per frame
    void Update()
    {
        /*// use wasd for camera movement (for right now)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // "cam.transform.right" is the horizontal screen axis
        // "cam.transform.up" is the vertical screen axis
        Vector3 move = (cam.transform.right * h) + (cam.transform.up * v); // vector for getting movement direction 

      
        if (move.magnitude > 1) move.Normalize(); // normalize to sytablize movement

        transform.position += move * panSpeed * Time.deltaTime; // update parent object through move vector and multiply speed and time for smoothness

        // We initialize a blank vector to store our intended movement
        Vector3 moveInput = Vector3.zero;*/

        Vector3 moveInput = Vector3.zero;

        // check key presses 
        if (Input.GetKey(KeyCode.I)) moveInput += cam.transform.up;    // move to top of screen
        if (Input.GetKey(KeyCode.K)) moveInput -= cam.transform.up;    // move to bottom of screen
        if (Input.GetKey(KeyCode.J)) moveInput -= cam.transform.right; // move to left of screen
        if (Input.GetKey(KeyCode.L)) moveInput += cam.transform.right; // move to right of screen

        // if moving normalize the vector so diagonal movement isn't faster
        if (moveInput.magnitude > 1)
        {
            moveInput.Normalize();
        }

        // Apply the movement to the parent "Iso Camera" object
        // multiplied by speed and deltaTime for frame-rate independence
        transform.position += moveInput * panSpeed * Time.deltaTime;
    }
}
