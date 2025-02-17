using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.up; // Default: rotate around the Y-axis
    public float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the object continuously
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
