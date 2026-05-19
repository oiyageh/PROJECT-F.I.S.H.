using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float crouchSpeed = 2f;
    public float rotationSpeedSmooth = 10f;

    [Header("Settings")]
    public float mouseSensitivity = 1f;

    [HideInInspector] public bool isPaused;

    [Header("Crouch")]
    public float standingHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchTransitionSpeed = 8f;

    [Header("References")]
    public Animator animator;

    [Header("Gravity")]
    public float gravity = -9.81f;
    private float verticalVelocity;

    private CharacterController controller;

    private bool isCrouching;

    private Vector3 originalCenter;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;

        controller.height = standingHeight;

        originalCenter = controller.center;
    }

    void Update()
    {
        if (isPaused)
            return;

        HandleCrouch();
        Move();
        ApplyGravity();
        HandleAnimations();
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
        }

        float targetHeight = isCrouching ? crouchHeight : standingHeight;

        controller.height = Mathf.Lerp(
            controller.height,
            targetHeight,
            Time.deltaTime * crouchTransitionSpeed
        );

        // Keeps player grounded 
        float centerY = controller.height / 2f;

        controller.center = new Vector3(
            originalCenter.x,
            centerY,
            originalCenter.z
        );
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Camera-relative movement
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
            float speed;

            if (isCrouching)
            {
                speed = crouchSpeed;
            }
            else
            {
                speed = Input.GetKey(KeyCode.LeftShift)
                    ? sprintSpeed
                    : walkSpeed;
            }

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

        bool isRunning = isMoving &&
                         Input.GetKey(KeyCode.LeftShift) &&
                         !isCrouching;

        float speedPercent;

        if (isCrouching && isMoving)
            speedPercent = 0.25f;
        else if (isRunning)
            speedPercent = 1f;
        else if (isMoving)
            speedPercent = 0.5f;
        else
            speedPercent = 0f;

        animator.SetFloat("Speed", speedPercent);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsCrouching", isCrouching);
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }
}