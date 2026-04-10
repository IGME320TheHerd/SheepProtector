using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float sprint = 15f;
    [SerializeField] Transform cam;

    Rigidbody rb;
    Vector2 moveInput = Vector2.zero;
    float currentSpeed;

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

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        currentSpeed = moveSpeed;
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        currentSpeed = sprint;
    }
}