using UnityEngine;

public class CameraRigFollow : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        if (player != null)
            transform.position = player.position;
    }
}
