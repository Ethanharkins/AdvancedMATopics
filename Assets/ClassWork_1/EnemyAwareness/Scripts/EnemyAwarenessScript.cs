using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAwarenessScript : MonoBehaviour, IAwareness
{
    public List<Collider> collidersInAwareness { get; set; }

    //
    public UnityAction<Collider> OnColliderEntersAwareness;
    //
    public UnityAction<Collider> OnColliderExitsAwareness;

    void Awake()
    {
        collidersInAwareness = new List<Collider>();
    }

    //When an object enters this trigger, we add it to the list
    private void OnTriggerEnter(Collider other)
    {
        AddToAwarenessList(other);
    }

    //When an object exits this trigger, we remove it from the list
    private void OnTriggerExit(Collider other)
    {
        RemoveFromAwarenessList(other);
    }

    //Adds the collider to the list
    public void AddToAwarenessList(Collider collider)
    {
        Debug.Log(collider.name + " entered awareness of this object.");
        collidersInAwareness.Add(collider);
        //After the item has been added to the list, we tell all subscribers of this event that it has entered awareness of this object
        //We add the ? to indicate that we will only invoke this event if it's not null
        OnColliderEntersAwareness?.Invoke(collider);
    }

    //Removes the collider from the list
    public void RemoveFromAwarenessList(Collider collider)
    {
        Debug.Log(collider.name + " left awareness of this object.");
        collidersInAwareness.Remove(collider);
        //After the item has been removed from the list, we tell all subscribers of this event that it has exited awareness of this object
        //We add the ? to indicate that we will only invoke this event
        OnColliderExitsAwareness?.Invoke(collider);
    }

    /// <summary>
    /// Returns true or false depending on whether there is a collider in range with the tag "tag". The resulting collider is returned as "col"
    /// </summary>
    /// <param name="tag">The tag we're searching for.</param>
    /// <param name="col">The first collider in awareness range to have that tag.</param>
    /// <returns></returns>
    public bool IsTagInRange(string tag, out Collider col)
    {
        //Stores in col the first collider that it finds with the matching tag
        col = collidersInAwareness.Find(x => x.CompareTag(tag));

        return col != null;
    }
}