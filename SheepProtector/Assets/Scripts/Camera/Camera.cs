using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class IsoCameraController : MonoBehaviour
{
    public Transform target; // refernce for player        
    public float rotationSpeed = 5f; // how fast the camera rotates
    //public InputActionMap cameraMap;

    private Camera cam; // refernce to camera
    private float targetYRotation; // refernce for roation angle

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>(); // gets the camera
        targetYRotation = transform.eulerAngles.y; // sets the rotation to default
    }

    void Update() // runs every frame after all update function to prevent camera issues
    {
        transform.position = target.position; // snaps pivot to player position

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetButtonDown("CameraTurnLeft")) targetYRotation -= 45f; // rotates left 45 degrees on left arrow/shoulder press
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetButtonDown("CameraTurnRight")) targetYRotation += 45f; // rotates 45 degrees right on right arrow/shoulder press

        Quaternion targetRotation = Quaternion.Euler(0, targetYRotation, 0); // creates quaternion value using y axis, left or right

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed); // rotates from current rotation to target rotation 
    }
}
