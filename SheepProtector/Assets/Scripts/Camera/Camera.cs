using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IsoCameraController : MonoBehaviour
{
    public Transform target; // refernce for player        
    public float rotationSpeed = 5f; // how fast the camera rotates

    private Camera cam; // refernce to camera
    private float targetYRotation = 0f; // refernce for roation angle

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>(); // gets the camera
        targetYRotation = transform.eulerAngles.y; // sets the rotation to default
    }

    void LateUpdate() // runs every frame after all update function to prevent camera issues
    {
        transform.position = target.position; // snaps pivot to player position

        Quaternion targetRotation = Quaternion.Euler(0, targetYRotation, 0); // creates quaternion value using y axis, left or right
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed); // rotates from current rotation to target rotation 
    }

    public void OnCameraMoveLeft(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !ctx.performed)
        {
            targetYRotation -= 45f;
        }
    }

    public void OnCameraMoveRight(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !ctx.performed)
        {
            targetYRotation += 45f;
        }
    }
}
