using UnityEngine;
using UnityEngine.InputSystem;

public class IsoCameraController : MonoBehaviour
{
    [SerializeField] private Transform target; // refernce for player        
    public float rotationSpeed = 5f; // how fast the camera rotates

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

        //if (Input.GetKeyDown(KeyCode.LeftArrow)) targetYRotation -= 45f; // rotates left 45 degrees on q press
        //if (Input.GetKeyDown(KeyCode.RightArrow)) targetYRotation += 45f; // rotates 45 degrees right on e press

        //Debug.Log(targetYRotation);

        Quaternion targetRotation = Quaternion.Euler(0, targetYRotation, 0); // creates quaternion value using y axis, left or right
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed); // rotates from current rotation to target rotation 
    }

    void CameraUpdate(float y)
    {
        targetYRotation += y;
        Quaternion targetRotation = Quaternion.Euler(0, targetYRotation, 0); // creates quaternion value using y axis, left or right
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed); // rotates from current rotation to target rotation 
    }

    public void OnCameraMoveLeft(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !ctx.performed)
        {
            targetYRotation -= 45f;
            Debug.Log(targetYRotation);
            //CameraUpdate(-45f);
           // Debug.Log("final movement: " + transform.rotation.ToString());
        }
    }

    public void OnCameraMoveRight(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !ctx.performed)
        {
            targetYRotation += 45f;
            Debug.Log(targetYRotation);
            //CameraUpdate(45f);
          //  Debug.Log("final movement: " + transform.rotation.ToString());
        }
    }
}
