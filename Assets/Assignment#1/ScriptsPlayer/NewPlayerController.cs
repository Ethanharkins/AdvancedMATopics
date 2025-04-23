using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NewPlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 velocity;
    private Animator animator;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.2f;

    [Header("Camera Reference")]
    public CameraOrbit cameraOrbit; // Drag your Main Camera here

    [Header("Flashlight")]
    public Light flashlight;
    private bool isFlashlightOn = false;

    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (flashlight != null)
            flashlight.enabled = false;
    }

    void Update()
    {
        Move();
        HandleJump();
        HandleFlashlight();

        // DEBUG: Detect unexpected reset of flashlight animation
        if (isFlashlightOn && !animator.GetBool("IsPointingFlashlight"))
        {
            Debug.LogWarning("⚠️ Animator bool 'IsPointingFlashlight' was reset externally!");
        }

        // 🏃 Set IsRunning based on Left Shift
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("IsRunning", isRunning);
    }

    void Move()
    {
        if (isFlashlightOn) return;

        isGrounded = controller.isGrounded;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(h, 0, v).normalized;
        Quaternion camYaw = cameraOrbit.GetCameraYawRotation();

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (input.magnitude >= 0.1f)
        {
            Vector3 moveDir = camYaw * input;
            controller.Move(moveDir * currentSpeed * Time.deltaTime);

            // Rotate to match camera
            transform.rotation = Quaternion.Slerp(transform.rotation, camYaw, Time.deltaTime * 10f);

            Vector3 localMove = Quaternion.Inverse(camYaw) * moveDir;
            animator.SetFloat("MoveX", localMove.x, 0.1f, Time.deltaTime);
            animator.SetFloat("MoveZ", localMove.z, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("MoveX", 0f, 0.1f, Time.deltaTime);
            animator.SetFloat("MoveZ", 0f, 0.1f, Time.deltaTime);
        }
    }

    void HandleJump()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFlashlightOn = !isFlashlightOn;
            Debug.Log("🟡 Flashlight Toggled: " + isFlashlightOn);

            if (flashlight != null)
                flashlight.enabled = isFlashlightOn;

            if (animator != null)
            {
                animator.SetBool("IsPointingFlashlight", isFlashlightOn);
                Debug.Log("🎬 SetBool IsPointingFlashlight = " + isFlashlightOn);
            }
        }
    }
}
