using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck<T> : List<T>
{
    /// <summary>
    /// Shuffles the list.
    /// </summary>
    public void Shuffle()
    {
        T temp;

        //For each element of this list
        for (int i = 0; i < this.Count; i++)
        {
            //Generate a random index to swap it with
            int rng = Random.Range(0, this.Count);

            //If the value isn't equal to the element we're currently looking to shuffle, we swap both elements
            if (rng != i)
            {
                temp = this[rng];
                this[rng] = this[i];
                this[i] = temp;
            }
        }
    }

}