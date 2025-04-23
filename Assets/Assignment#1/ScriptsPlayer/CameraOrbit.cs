using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // Assign CameraRig (which follows the player)
    public Vector3 offset = new Vector3(0, 3, -5);
    public float mouseSensitivity = 3f;

    private float yaw = 0f;
    private float pitch = 10f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -40f, 80f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 position = rotation * offset + target.position;

        transform.position = position;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }

    public Quaternion GetCameraYawRotation()
    {
        return Quaternion.Euler(0f, yaw, 0f);
    }
}
