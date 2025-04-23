using UnityEngine;

public class CameraFollowTargetPositionOnly : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        transform.position = player.position;
    }
}
