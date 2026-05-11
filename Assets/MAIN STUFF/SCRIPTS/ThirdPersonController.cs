using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float rotationSpeedSmooth = 10f;

    [Header("References")]
    public Animator animator;

    [Header("Gravity")]
    public float gravity = -9.81f;
    private float verticalVelocity;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        ApplyGravity();
        HandleAnimations();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Camera-relative movement (Cinemachine uses Main Camera)
        Transform cam = Camera.main.transform;

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * v + right * h;

        if (moveDir.magnitude >= 0.1f)
        {
            // Rotate player toward movement direction
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;

            float angle = Mathf.LerpAngle(
                transform.eulerAngles.y,
                targetAngle,
                Time.deltaTime * rotationSpeedSmooth
            );

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Movement speed
            float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    void HandleAnimations()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool isMoving = new Vector2(h, v).magnitude > 0.1f;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        float speedPercent = isRunning ? 1f : (isMoving ? 0.5f : 0f);

        animator.SetFloat("Speed", speedPercent);
        animator.SetBool("IsRunning", isRunning);
    }
}