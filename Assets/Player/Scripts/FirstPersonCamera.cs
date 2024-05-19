using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    #region Variables
    public float sensX;

    public float sensY;

    [SerializeField]
    private Transform orientation;

    [SerializeField]
    private Transform player;

    private float xRotation;
    private float yRotation;
    #endregion Variables

    #region Lifecycle
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mouseInputs = PlayerController.Instance.lookAction.ReadValue<Vector2>().normalized;
        float mouseX = mouseInputs.x * sensX;
        float mouseY = mouseInputs.y * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        player.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    #endregion Lifecycle
}
