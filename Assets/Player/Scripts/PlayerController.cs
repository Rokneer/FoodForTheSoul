using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    #region Variables
    internal PlayerInput playerInput;
    private Rigidbody rb;

    private PhotoCapture photoCapture;
    private PhotoZoom photoZoom;
    private PhotoFlash photoFlash;

    private InputAction moveAction;
    private InputAction photoAction;
    private InputAction zoomAction;
    private InputAction extraZoomAction;
    private InputAction flashAction;
    private InputAction reloadAction;
    private InputAction runAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction interactAction;
    private InputAction pauseAction;

    [Header("Player")]
    [SerializeField]
    private GameObject playerObject;

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

    private bool HasCeilingAbove =>
        Physics.Raycast(transform.position, Vector3.up, playerHeight * 1f + 0.3f);

    [Header("Ground Check")]
    [SerializeField]
    private float playerHeight;

    private bool IsGrounded =>
        Physics.Raycast(
            transform.position,
            Vector3.down,
            (playerHeight * 0.5f) + 0.3f,
            groundLayer
        );

    [SerializeField]
    private LayerMask groundLayer;

    [Header("Slope Handling")]
    private bool isExitingSlope;
    private bool IsOnSlope
    {
        get
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
    }
    private Vector3 SlopeMoveDirection =>
        Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;

    [SerializeField]
    private float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Header("Camera Orientation")]
    [SerializeField]
    private Transform orientation;

    [Header("Interactable")]
    [SerializeField]
    internal bool isInsideInteractable = false;
    internal event Action InteractableAction;

    [Header("Audio")]
    [SerializeField]
    private AudioClip rechargeSound;

    #endregion

    #region Lifecycle
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        // Photo functionality
        photoCapture = GetComponentInChildren<PhotoCapture>();
        photoZoom = GetComponentInChildren<PhotoZoom>();
        photoFlash = GetComponentInChildren<PhotoFlash>();

        // Defines input actions based on the Player Input component
        moveAction = playerInput.actions[PlayerActionStrings.Move];
        photoAction = playerInput.actions[PlayerActionStrings.Photo];
        zoomAction = playerInput.actions[PlayerActionStrings.Zoom];
        extraZoomAction = playerInput.actions[PlayerActionStrings.ExtraZoom];
        flashAction = playerInput.actions[PlayerActionStrings.Flash];
        reloadAction = playerInput.actions[PlayerActionStrings.Reload];
        runAction = playerInput.actions[PlayerActionStrings.Run];
        jumpAction = playerInput.actions[PlayerActionStrings.Jump];
        crouchAction = playerInput.actions[PlayerActionStrings.Crouch];
        interactAction = playerInput.actions[PlayerActionStrings.Interact];
        pauseAction = playerInput.actions[PlayerActionStrings.Pause];
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
        extraZoomAction.started += OnExtraZoom;
        flashAction.started += OnFlash;
        reloadAction.started += OnReload;
        runAction.started += OnRun;
        runAction.canceled += OnRun;
        jumpAction.started += OnJump;
        crouchAction.started += OnCrouch;
        crouchAction.canceled += OnCrouch;
        interactAction.started += OnInteract;
        pauseAction.started += OnPause;
    }

    private void OnDisable()
    {
        // Unsuscribes input actions to its respective functions to prevent memory leaks
        photoAction.started -= OnPhoto;
        zoomAction.started -= OnZoom;
        extraZoomAction.started -= OnExtraZoom;
        flashAction.started -= OnFlash;
        reloadAction.started -= OnReload;
        runAction.started -= OnRun;
        runAction.canceled -= OnRun;
        jumpAction.started -= OnJump;
        crouchAction.started -= OnCrouch;
        crouchAction.canceled -= OnCrouch;
        interactAction.started -= OnInteract;
        pauseAction.started -= OnPause;
    }

    private void Update()
    {
        // Manages player speed
        ManageSpeed();

        // Handles applying drag
        rb.drag = IsGrounded ? groundDrag : 0;
    }

    private void FixedUpdate()
    {
        // Checks for input to move the player
        Movement();
    }
    #endregion

    #region Movement
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
        if (IsOnSlope && !isExitingSlope)
        {
            rb.AddForce(20f * MovementSpeed * SlopeMoveDirection, ForceMode.Force);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        // Force added on a even ground
        else if (IsGrounded)
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

        rb.useGravity = !IsOnSlope;
    }

    private void ManageSpeed()
    {
        // Limits speed on a slope
        if (IsOnSlope && !isExitingSlope && rb.velocity.magnitude > MovementSpeed)
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
    #endregion

    #region Crouch
    private void OnCrouch(InputAction.CallbackContext context)
    {
        if (IsGrounded && !PauseManager.Instance.IsPaused)
        {
            if (context.started)
            {
                movementState = MovementState.Crouching;
                playerObject.transform.localScale = new(
                    playerObject.transform.localScale.x,
                    crouchYScale,
                    playerObject.transform.localScale.z
                );
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            }

            if (context.canceled && !HasCeilingAbove)
            {
                movementState = MovementState.Walking;
                playerObject.transform.localScale = new(
                    playerObject.transform.localScale.x,
                    originalYScale,
                    playerObject.transform.localScale.z
                );
            }
        }
    }
    #endregion

    #region Run
    private void OnRun(InputAction.CallbackContext context)
    {
        if (
            IsGrounded
            && movementState != MovementState.Crouching
            && !PauseManager.Instance.IsPaused
        )
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
    #endregion

    #region Jump
    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isReadyToJump && IsGrounded && !PauseManager.Instance.IsPaused)
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
    #endregion

    #region Interact
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && isInsideInteractable && !PauseManager.Instance.IsPaused)
        {
            InteractableAction?.Invoke();
        }
    }
    #endregion

    #region Flash
    private void OnFlash(InputAction.CallbackContext context)
    {
        if (context.started && !PauseManager.Instance.IsPaused)
        {
            StartCoroutine(photoFlash.ActivateFlash());
        }
    }
    #endregion

    #region Reload
    private void OnReload(InputAction.CallbackContext context)
    {
        if (context.started && !PauseManager.Instance.IsPaused)
        {
            PhotoCameraUIManager.Instance.ResetPhotos();
            SoundFXManager.Instance.PlaySFXClip(rechargeSound, transform, 0.5f, true);
        }
    }
    #endregion

    #region Zoom
    private void OnZoom(InputAction.CallbackContext context)
    {
        if (context.started && !PauseManager.Instance.IsPaused)
        {
            photoZoom.IsZooming = !photoZoom.IsZooming;
            if (photoZoom.IsZooming)
            {
                photoZoom.ZoomIn();
            }
            else
            {
                photoZoom.ZoomOut();
            }
        }
    }

    private void OnExtraZoom(InputAction.CallbackContext context)
    {
        if (context.started && photoZoom.IsZooming && !PauseManager.Instance.IsPaused)
        {
            photoZoom.isExtraZooming = !photoZoom.isExtraZooming;
            if (photoZoom.isExtraZooming)
            {
                photoZoom.ExtraZoomIn();
            }
            else
            {
                photoZoom.ZoomIn();
            }
        }
    }
    #endregion

    #region Photo
    private void OnPhoto(InputAction.CallbackContext context)
    {
        if (context.started && photoCapture.canTakePhoto && !PauseManager.Instance.IsPaused)
        {
            StartCoroutine(photoCapture.CapturePhoto());
        }
    }
    #endregion

    #region Pause
    private void OnPause(InputAction.CallbackContext context)
    {
        if (context.started && PauseManager.Instance.canPause)
        {
            PauseManager.Instance.ManagePauseMenu();
        }
    }
    #endregion
}
