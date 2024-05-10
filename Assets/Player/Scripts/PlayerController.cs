using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private static PlayerController _instance;
    public static PlayerController Instance => _instance;

    private PlayerInput playerInput;
    private Rigidbody rb;
    private InputAction moveAction;

    [HideInInspector]
    public InputAction lookAction;
    private InputAction photoAction;
    private InputAction zoomAction;
    private InputAction flashAction;
    private InputAction runAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction interactAction;

    [Header("Movement")]
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float sprintSpeed;
    private float MovementSpeed =>
        movementState switch
        {
            MovementState.Running => sprintSpeed,
            MovementState.Crouching => crouchSpeed,
            _ => walkSpeed,
        };

    [SerializeField]
    private float groundDrag;
    private Vector3 moveDirection;

    [SerializeField]
    private MovementState movementState;

    private enum MovementState
    {
        Walking,
        Running,
        Crouching,
        Air
    }

    [Header("Jump")]
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float jumpCooldown;

    [SerializeField]
    private float airMultipler;
    private bool isReadyToJump = true;

    [Header("Crouch")]
    [SerializeField]
    private float crouchSpeed;

    [SerializeField]
    private float crouchYScale;
    private float originalYScale;

    [Header("Ground Check")]
    [SerializeField]
    private float playerHeight;

    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private LayerMask groundLayer;

    [Header("Slope Handling")]
    [SerializeField]
    private bool isOnSlope;
    private bool isExitingSlope;

    [SerializeField]
    private float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Space]
    [SerializeField]
    private Transform orientation;

    #endregion Variables

    #region Lifecycle
    private void Awake()
    {
        // Checks if there is only one instance of the script in the scene
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        // Defines input actions based on the Player Input component
        moveAction = playerInput.actions[PlayerActionStrings.Move];
        lookAction = playerInput.actions[PlayerActionStrings.Look];
        photoAction = playerInput.actions[PlayerActionStrings.Photo];
        zoomAction = playerInput.actions[PlayerActionStrings.Zoom];
        flashAction = playerInput.actions[PlayerActionStrings.Flash];
        runAction = playerInput.actions[PlayerActionStrings.Run];
        jumpAction = playerInput.actions[PlayerActionStrings.Jump];
        crouchAction = playerInput.actions[PlayerActionStrings.Crouch];
        interactAction = playerInput.actions[PlayerActionStrings.Interact];
    }

    private void Start()
    {
        // Locks cursor to the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Saves the original Y scale for the player
        originalYScale = transform.localScale.y;

        // Freezes player rotation
        rb.freezeRotation = true;

        // Suscribes input actions to its respective functions
        photoAction.started += OnPhoto;
        zoomAction.started += OnZoom;
        flashAction.started += OnFlash;
        runAction.started += OnRun;
        runAction.canceled += OnRun;
        jumpAction.started += OnJump;
        crouchAction.started += OnCrouch;
        crouchAction.canceled += OnCrouch;
        interactAction.started += OnInteract;
    }

    private void OnDisable()
    {
        // Unsuscribes input actions to its respective functions to prevent memory leaks
        photoAction.started -= OnPhoto;
        zoomAction.started -= OnZoom;
        flashAction.started -= OnFlash;
        runAction.started -= OnRun;
        runAction.canceled -= OnRun;
        jumpAction.started -= OnJump;
        crouchAction.started -= OnCrouch;
        crouchAction.canceled -= OnCrouch;
        interactAction.started -= OnInteract;
    }

    private void Update()
    {
        // Checks if player is grounded
        isGrounded = CheckIfGrounded();

        // Checks if player is on a slope
        isOnSlope = CheckIfOnSlope();

        // Manages player speed
        ManageSpeed();

        // Handles applying drag
        rb.drag = isGrounded ? groundDrag : 0;
    }

    private void FixedUpdate()
    {
        // Checks for input to move the player
        Movement();
    }
    #endregion Lifecycle

    #region Functions
    private void Movement()
    {
        // Gets value from movement input action
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float horizontalInput = moveInput.x;
        float verticalInput = moveInput.y;

        // Calculates move direction
        moveDirection =
            (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        // Force added on a slope
        if (isOnSlope && !isExitingSlope)
        {
            rb.AddForce(20f * MovementSpeed * GetSlopeMoveDirection(), ForceMode.Force);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        // Force added on a even ground
        else if (isGrounded)
        {
            rb.AddForce(10f * MovementSpeed * moveDirection.normalized, ForceMode.Force);
        }
        // Force added in the air
        else
        {
            rb.AddForce(
                10f * airMultipler * MovementSpeed * moveDirection.normalized,
                ForceMode.Force
            );
        }

        rb.useGravity = !isOnSlope;
    }

    private void ManageSpeed()
    {
        // Limits speed on a slope
        if (isOnSlope && !isExitingSlope && rb.velocity.magnitude > MovementSpeed)
        {
            rb.velocity = rb.velocity.normalized * MovementSpeed;
        }
        // Limits speed on even ground or the air
        else
        {
            Vector3 flatVelocity = new(rb.velocity.x, 0f, rb.velocity.z);

            // Limit velocity when needed
            if (flatVelocity.magnitude > MovementSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * MovementSpeed;
                rb.velocity = new(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact");
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            if (context.started)
            {
                movementState = MovementState.Crouching;
                transform.localScale = new(
                    transform.localScale.x,
                    crouchYScale,
                    transform.localScale.z
                );
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            }

            if (context.canceled)
            {
                bool hasCeilingAbove = Physics.Raycast(
                    transform.position,
                    Vector3.up,
                    playerHeight * 0.5f + 0.3f
                );
                if (!hasCeilingAbove)
                {
                    movementState = MovementState.Walking;
                    transform.localScale = new(
                        transform.localScale.x,
                        originalYScale,
                        transform.localScale.z
                    );
                }
            }
        }
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            if (context.started)
            {
                movementState = MovementState.Running;
            }
            if (context.canceled)
            {
                movementState = MovementState.Walking;
            }
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isReadyToJump && isGrounded)
        {
            isReadyToJump = false;
            isExitingSlope = true;
            rb.velocity = new(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        isReadyToJump = true;
        isExitingSlope = false;
    }

    private void OnFlash(InputAction.CallbackContext context)
    {
        Debug.Log("Flash");
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        Debug.Log("Zoom");
    }

    private void OnPhoto(InputAction.CallbackContext context)
    {
        Debug.Log("Photo");
    }

    private bool CheckIfGrounded()
    {
        return Physics.Raycast(
            transform.position,
            Vector3.down,
            (playerHeight * 0.5f) + 0.3f,
            groundLayer
        );
    }

    private bool CheckIfOnSlope()
    {
        bool isOnSlope = Physics.Raycast(
            transform.position,
            Vector3.down,
            out slopeHit,
            playerHeight * 0.5f + 0.3f
        );

        if (isOnSlope)
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    #endregion Functions
}
