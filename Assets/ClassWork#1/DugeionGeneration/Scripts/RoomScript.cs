using UnityEngine;
using System.Collections.Generic;

public class RoomScript : MonoBehaviour
{
    [System.Serializable]
    public class ExitClass
    {
        public string name;
        public Transform alignmentObject;
        public List<string> keywords = new List<string>();

        public bool CanConnect(ExitClass other)
        {
            foreach (var kw in other.keywords)
            {
                if (this.keywords.Contains(kw))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public ExitClass[] exits;

    void Start()
    {
        if (exits.Length == 0)
        {
            Debug.LogError("Room " + gameObject.name + " has no exits!");
        }
        else
        {
            foreach (var exit in exits)
            {
                if (exit.alignmentObject == null)
                {
                    Debug.LogError("Exit " + exit.name + " in room " + gameObject.name + " has no alignment object assigned!");
                }
            }
        }
    }

    public bool TryConnect(ExitClass localExit, ExitClass otherExit)
    {
        if (localExit == null || otherExit == null)
        {
            Debug.LogError("TryConnect failed: one of the exits is null!");
            return false;
        }

        if (!localExit.CanConnect(otherExit))
        {
            Debug.Log("Exits " + localExit.name + " and " + otherExit.name + " do not share a common keyword.");
            return false;
        }

        Transform alignedParent = localExit.alignmentObject.parent;
        Transform roomParent = this.transform.parent;

        localExit.alignmentObject.SetParent(roomParent);
        this.transform.SetParent(localExit.alignmentObject);

        localExit.alignmentObject.position = otherExit.alignmentObject.position;
        localExit.alignmentObject.forward = -otherExit.alignmentObject.forward;

        this.transform.SetParent(roomParent);
        localExit.alignmentObject.SetParent(alignedParent);

        return CheckOverlap(GetComponentInChildren<Collider>());
    }

    public bool CheckOverlap(Collider thisRoomCollider)
    {
        if (thisRoomCollider == null)
        {
            Debug.LogError("CheckOverlap() failed: thisRoomCollider is null in room " + gameObject.name);
            return false;
        }

        Vector3 colliderDimensions = thisRoomCollider.bounds.extents;
        thisRoomCollider.enabled = false;
        bool isOverlapping = Physics.CheckBox(transform.position, colliderDimensions);
        thisRoomCollider.enabled = true;

        return !isOverlapping;
    }
}
