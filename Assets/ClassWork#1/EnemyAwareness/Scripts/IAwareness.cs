using UnityEngine;
//Included so we can use the List<> data container
using System.Collections.Generic;

public interface IAwareness
{
    public List<Collider> collidersInAwareness
    {
        get;
        set;
    }



}