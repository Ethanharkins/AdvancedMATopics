using UnityEngine;

public class FreeLookCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;
    public float sensitivity = 3f;
    public float height = 2f;

    private float currentX = 0f;
    private float currentY = 0f;
    public float minY = -20f;
    public float maxY = 60f;

    void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;
        currentY = Mathf.Clamp(currentY, minY, maxY);

        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + rotation * dir + Vector3.up * height;
        transform.LookAt(target.position + Vector3.up * height);
    }
}
