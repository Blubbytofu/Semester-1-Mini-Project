using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject orientation;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private LayerMask groundMask;

    public float forwardInput { get; private set; }
    public float sideInput { get; private set; }
    public bool isRunning;
    private Vector3 walkDirection;
    private Vector3 slopeMoveDirection;

    private float movementSpeed;
    private RaycastHit slopeHit;
    private float playerHeight = 1.8f;

    private float gravityMultiplier = 1.5f;
    private float jumpForce = 500f;
    private float groundedSphereRadius = 0.2f;
    public bool isGrounded { get; private set; }
    private bool jumping = false;
    private float nextJumpTime;
    private float jumpCooldown = 0.2f;

    private void Start()
    {
        orientation = GameObject.Find("Orientation");
        playerRb = GetComponent<Rigidbody>();
        groundMask = LayerMask.GetMask("Environment");

        Physics.gravity *= gravityMultiplier;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundedSphereRadius, groundMask);

        MovementInput();
        ControlDragAndSpeed();

        JumpInput();
    }

    private void FixedUpdate()
    {
        Jump();
        Move();
    }

    private void JumpInput()
    {
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Time.time > nextJumpTime)
                {
                    jumping = true;
                    nextJumpTime = Time.time + jumpCooldown;
                }
            }
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, playerHeight / 2f, 0), Vector3.down, out slopeHit, playerHeight / 2 + 0.2f, groundMask))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        return false;
    }

    private void ControlDragAndSpeed()
    {
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = 30f;
                playerRb.drag = 3f;
                isRunning = true;
            }
            else
            {
                movementSpeed = 15f;
                playerRb.drag = 5f;
                isRunning = false;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = 30f;
                playerRb.drag = 0.5f;
            }
            else
            {
                movementSpeed = 10f;
                playerRb.drag = 0.5f;
            }
        }

        if (OnSlope() && forwardInput == 0 && sideInput == 0)
        {
            playerRb.drag = 100f;
        }
    }

    private void MovementInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            forwardInput = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            forwardInput = -1;
        }
        else
        {
            forwardInput = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            sideInput = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            sideInput = 1;
        }
        else
        {
            sideInput = 0;
        }
    }

    private void Move()
    {
        walkDirection = orientation.transform.forward * forwardInput + orientation.transform.right * sideInput;
        slopeMoveDirection = Vector3.ProjectOnPlane(walkDirection, slopeHit.normal);

        if (isGrounded)
        {
            if (OnSlope())
            {

                playerRb.AddForce(slopeMoveDirection.normalized * movementSpeed, ForceMode.Acceleration);
            }
            else
            {

                playerRb.AddForce(walkDirection.normalized * movementSpeed, ForceMode.Acceleration);
            }
        }
    }

    private void Jump()
    {
        if (jumping)
        {
            playerRb.AddForce(new Vector3(0, 1, 0) * jumpForce, ForceMode.Impulse);
            jumping = false;
        }
    }
}
