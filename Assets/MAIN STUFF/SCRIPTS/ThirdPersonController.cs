using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float rotationSpeed = 10f;

    [Header("References")]
    public Transform cameraTransform;
    public Animator animator;

    private CharacterController controller;
    public float gravity = -9.81f;
    private float verticalVelocity;


    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // Prevent physics from rotating player
        controller.detectCollisions = true;
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 gravityMove = Vector3.up * verticalVelocity;
        controller.Move(gravityMove * Time.deltaTime);
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

        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            // Camera-relative direction
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // Smooth rotation
            float angle = Mathf.SmoothDampAngle(
              transform.eulerAngles.y,
              targetAngle,
              ref rotationSpeed,
              0.1f
 );
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Move direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Sprint
            float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    void HandleAnimations()
    {
        // Get movement input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Check if player is moving
        bool isMoving = new Vector2(h, v).magnitude > 0.1f;

        // Check if sprinting
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        // Animation values
        float speedPercent = isRunning ? 1f : (isMoving ? 0.5f : 0f);

        // Send to Animator
        animator.SetFloat("Speed", speedPercent);
        animator.SetBool("IsRunning", isRunning);
    }
}