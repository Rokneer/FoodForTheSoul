using UnityEngine;

public class RotateInPlace : MonoBehaviour
{
    [SerializeField]
    [Range(0, 200)]
    private float rotationSpeed = 100;

    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up);
    }
}
