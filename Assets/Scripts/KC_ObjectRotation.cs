using UnityEngine;

public class KC_ObjectRotation : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation
    public float smoothTime = 0.1f; // Smoothness of rotation
    private Quaternion targetRotation;
    private bool isRotating = false;
    private Camera playerCamera;

    void Start()
    {
        targetRotation = transform.rotation;
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothTime);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                isRotating = false;
            }
        }

        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
            {
                targetRotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
                isRotating = true;
            }
        }
    }
}