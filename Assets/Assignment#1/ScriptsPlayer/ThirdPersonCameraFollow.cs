using UnityEngine;

public class ThirdPersonCameraFollow : MonoBehaviour
{
    public Transform target; // Assign CameraFollowTarget here
    public Vector3 offset = new Vector3(0, 3, -5);
    public float rotationSpeed = 1.5f;

    float currentX = 0f;
    float currentY = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // Use raw mouse delta instead of Input.GetAxis
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        currentX += mouseX * rotationSpeed;
        currentY -= mouseY * rotationSpeed;
        currentY = Mathf.Clamp(currentY, -40, 80);

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 direction = rotation * offset;

        transform.position = target.position + direction;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
