using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 120f;
    public float speed = 10f;
    private float angle = 0f;

    void LateUpdate()
    {
        angle += speed * Time.deltaTime;
        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;
        transform.position = new Vector3(x, 50f, z);
        transform.LookAt(target.position);
    }
}
