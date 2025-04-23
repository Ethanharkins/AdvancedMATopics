using UnityEngine;
//
using UnityEngine.Events;

public class EnemyHearingScript : MonoBehaviour
{
    //
    public bool canHearThroughWalls = true;
    //
    public UnityAction<SoundClass> OnSoundHeard;

    //
    public void CheckIfHeard(SoundClass sound)
    {
        Debug.Log("I heard something");

        if (canHearThroughWalls)
        {
            OnSoundHeard?.Invoke(sound);
            return;
        }
        else
        {
            Debug.DrawLine(sound.position, this.transform.position, Color.green);
            if (Physics.Raycast(sound.position, (this.transform.position - sound.position).normalized, out RaycastHit hitInfo))
            {
                if (hitInfo.transform != this.transform)
                {
                    return;
                }

                OnSoundHeard?.Invoke(sound);
            }
        }
    }
}