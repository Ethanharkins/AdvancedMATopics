using UnityEngine;

public class WallScript : MonoBehaviour
{
    public MeshRenderer wallRenderer;
    public string[] possibleExits;

    public bool TryConnect(RoomScript.ExitClass exit)
    {
        if (exit.alignmentObject == null)
        {
            Debug.LogError("Wall cannot connect: Exit " + exit.name + " has no alignment object!");
            return false;
        }

        Debug.Log("Trying to connect wall to exit: " + exit.name + " with possible exits: " + string.Join(", ", possibleExits));

        foreach (string keyword in possibleExits)
        {
            if (exit.keywords.Contains(keyword))
            {
                transform.position = exit.alignmentObject.position;

                // Ensure the wall faces inward and apply a 90-degree rotation correction
                transform.rotation = Quaternion.LookRotation(-exit.alignmentObject.forward) * Quaternion.Euler(0, 90, 0);

                Debug.Log("Wall successfully connected to exit: " + exit.name + " at position: "
                          + transform.position + " with rotation: " + transform.rotation.eulerAngles);
                return true;
            }
        }

        Debug.LogWarning("Wall failed to connect to exit: " + exit.name + ". Exit keywords: " + string.Join(", ", exit.keywords));
        return false;
    }
}
