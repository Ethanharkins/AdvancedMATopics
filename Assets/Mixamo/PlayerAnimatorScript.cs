using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Set movement parameters
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        // Flashlight point (hold right mouse button)
        bool isPointing = Input.GetMouseButton(1); // or use Input.GetKey(KeyCode.F) if you prefer
        animator.SetBool("IsPointingFlashlight", isPointing);
    }
}
