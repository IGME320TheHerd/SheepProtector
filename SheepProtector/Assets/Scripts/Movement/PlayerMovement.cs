using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float moveSpeed = 10f; // movement speed

    private Rigidbody rb; // physics component reference

    private Vector3 moveDir; // reference to movement direction

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // link rigid body component
    }

    // Update is called once per frame
    void Update()
    {
        // get input
        float h = Input.GetAxisRaw("Horizontal"); 
        float v = Input.GetAxisRaw("Vertical");

        Vector3 forward = new Vector3(1, 0, 1).normalized; //forward is up right diagnol
        Vector3 right = new Vector3(1, 0, -1).normalized; // right is down right diagnol

       moveDir = (forward * v) + (right * h); // combines vectors for w s for 'forward' and a d for 'right
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveDir.x * moveSpeed,rb.linearVelocity.y, moveDir.z * moveSpeed); // apply movement to rigid body
    }
}
