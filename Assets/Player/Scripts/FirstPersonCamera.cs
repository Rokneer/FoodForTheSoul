using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    #region Variables
    internal float SenX
    {
        get => SettingsMenu.Instance.SensitivityX;
        set => SettingsMenu.Instance.SensitivityX = value;
    }

    internal float SenY
    {
        get => SettingsMenu.Instance.SensitivityY;
        set => SettingsMenu.Instance.SensitivityY = value;
    }

    [Header("Positions")]
    [SerializeField]
    private Transform orientation;

    [SerializeField]
    private Transform player;
    private InputAction lookAction;

    private float xRotation;
    private float yRotation;
    #endregion

    #region Lifecycle
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        lookAction = PlayerController.Instance.playerInput.actions[PlayerActionStrings.Look];
    }

    private void Update()
    {
        if (!PauseManager.Instance.IsPaused)
        {
            Vector2 mouseInput = lookAction.ReadValue<Vector2>().normalized;
            float mouseX = mouseInput.x * SenX;
            float mouseY = mouseInput.y * SenY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            player.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
    #endregion
}
