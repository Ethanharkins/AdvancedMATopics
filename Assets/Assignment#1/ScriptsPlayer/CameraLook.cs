using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseSensitivity = 1000f;
    public Transform playerBody; // This is now just the root the camera follows

    private float xRotation = 0f;
    private bool isPaused = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isPaused) return; // Stop camera movement when paused

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Removed rotation of player
        // playerBody.Rotate(Vector3.up * mouseX);
    }
}
