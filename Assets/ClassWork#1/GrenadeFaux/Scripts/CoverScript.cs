using UnityEngine;

public class CoverScript : MonoBehaviour
{
    public LayerMask blockingLayers;

    /// <summary>
    /// Returns whether the position of this node and the other vector given see each other without an obstacle on the way
    /// </summary>
    /// <param name="otherPosition"></param>
    /// <returns></returns>
    public bool SeesPositionUninterrupted(Vector3 otherPosition)
    {

        Ray ray = new Ray(this.transform.position,
            (otherPosition - this.transform.position));

        if (Physics.Raycast(ray,
            out RaycastHit hitInfo,
            Vector3.Distance(otherPosition, this.transform.position),
            blockingLayers))
        {
            return false;
        }

        return true;
    }
}