using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction pointerPositionAction;
    private InputAction photoAction;
    private InputAction zoomAction;
    private InputAction flashAction;
    private InputAction runAction;
    private InputAction crouchAction;
    private InputAction interactAction;

    private Vector2 moveInput;
    private Vector2 pointerInput;

    #endregion Variables

    #region Lifecycle
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        
        // Defines input actions based on the Player Input component
        moveAction = playerInput.actions[PlayerActionStrings.Move];
        pointerPositionAction = playerInput.actions[PlayerActionStrings.PointerPosition];
        photoAction = playerInput.actions[PlayerActionStrings.Photo];
        zoomAction = playerInput.actions[PlayerActionStrings.Zoom];
        flashAction = playerInput.actions[PlayerActionStrings.Flash];
        runAction = playerInput.actions[PlayerActionStrings.Run];
        crouchAction = playerInput.actions[PlayerActionStrings.Crouch];
        interactAction = playerInput.actions[PlayerActionStrings.Interact];
    }

    private void Start()
    {
        // Suscribes input actions to its respective functions
        photoAction.started += OnPhoto;
        zoomAction.started += OnZoom;
        flashAction.started += OnFlash;
        runAction.started += OnRun;
        crouchAction.started += OnCrouch;
        interactAction.started += OnInteract;
    }

    private void OnDisable()
    {
        // Unsuscribes input actions to its respective functions to prevent memory leaks
        photoAction.started -= OnPhoto;
        zoomAction.started -= OnZoom;
        flashAction.started -= OnFlash;
        runAction.started -= OnRun;
        crouchAction.started -= OnCrouch;
        interactAction.started -= OnInteract;
    }
    #endregion Lifecycle

    #region Functions
    private void OnInteract(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void OnFlash(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void OnPhoto(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
    #endregion Functions
}
