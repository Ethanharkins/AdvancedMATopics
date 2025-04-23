using UnityEngine;

public class PlayerIKHandler : MonoBehaviour
{
    public Transform lookTarget; // Camera

    [Header("Foot IK")]
    public LayerMask groundLayer;
    public float footRaycastDistance = 1.2f;
    public float footOffsetY = 0.1f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        // 👁️ Head LookAt
        if (lookTarget != null)
        {
            animator.SetLookAtWeight(1.0f, 0.3f, 1.0f, 1.0f, 0.5f);
            animator.SetLookAtPosition(lookTarget.position + lookTarget.forward * 10f);
        }

        // 🦶 Foot IK
        HandleFootIK(AvatarIKGoal.LeftFoot);
        HandleFootIK(AvatarIKGoal.RightFoot);
    }

    void HandleFootIK(AvatarIKGoal foot)
    {
        Vector3 footPos = animator.GetIKPosition(foot);
        Ray ray = new Ray(footPos + Vector3.up * 0.5f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, footRaycastDistance, groundLayer))
        {
            Vector3 targetPos = hit.point + Vector3.up * footOffsetY;
            Quaternion targetRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);

            animator.SetIKPositionWeight(foot, 1f);
            animator.SetIKRotationWeight(foot, 1f);

            animator.SetIKPosition(foot, targetPos);
            animator.SetIKRotation(foot, targetRot);
        }
        else
        {
            animator.SetIKPositionWeight(foot, 0f);
            animator.SetIKRotationWeight(foot, 0f);
        }
    }
}
