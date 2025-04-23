using UnityEngine;

public class SpeakerOnClickScript : MonoBehaviour
{
    //The sound that we will be playing
    public SO_SoundClip sound;
    //The source that will play that sound
    public AudioSource source;

    //A variable to keep track of the sound that was last played
    protected SoundClass newSound;

    //If there's no source assigned, get the one from this object
    protected void Awake()
    {
        if (sound == null)
            source = GetComponent<AudioSource>();
    }

    //A function that plays the sound on this object
    public virtual void PlaySound()
    {
        //Assign a new value to newSound
        newSound = new SoundClass(source, sound.clip, sound.range, this.transform.position);
        //Play it
        newSound.Play();
    }

    //When this object is clicked on
    protected void OnMouseDown()
    {
        PlaySound();
    }

    //
    protected void OnDrawGizmos()
    {
        if (newSound == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(newSound.position, newSound.range);

    }
}