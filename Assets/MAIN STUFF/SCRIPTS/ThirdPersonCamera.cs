using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 200f;
    public float minY = -35f;
    public float maxY = 60f;

    [Header("Camera Settings")]
    public float distance = 5f;
    public float height = 2f;

    [Header("Collision")]
    public float collisionRadius = 0.3f;
    public LayerMask collisionMask;
    public float minDistance = 1.5f;
    public float smoothSpeed = 10f;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private float currentDistance;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentDistance = distance;
    }

    void LateUpdate()
    {
        HandleRotation();
        HandleCollision();
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minY, maxY);
    }

    void HandleCollision()
    {
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        Vector3 direction = rotation * Vector3.back;

        Vector3 pivot = target.position + Vector3.up * height;

        RaycastHit hit;

        float targetDistance = distance;

        // SphereCast to detect walls
        if (Physics.SphereCast(pivot, collisionRadius, direction, out hit, distance, collisionMask))
        {
            targetDistance = Mathf.Clamp(hit.distance, minDistance, distance);
        }

        // Smooth distance change
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * smoothSpeed);

        Vector3 finalPosition = pivot + direction * currentDistance;

        transform.position = finalPosition;
        transform.LookAt(pivot);
    }
}