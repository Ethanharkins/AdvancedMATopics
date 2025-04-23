using System.Collections;
using UnityEngine;

public class ExplodeOnClickScript : SpeakerOnClickScript
{
    //The explosion of this explosive
    public ParticleSystem explosion;
    //The sound played to warn enemies of the explosion
    public SO_SoundClip warningSound;
    //The time between the warning and the explosion
    public float timeBeforeExplosion = 3f;
    //Whether this has exploded
    public bool hasExploded = false;

    //Plays the warning sound, then calls the coroutine to wait the given amount of time before playing the explosion
    public override void PlaySound()
    {
        //Plays a sound to warn the enemies to take cover
        newSound = new WarningSoundClass(source, warningSound.clip, warningSound.range, this.transform.position, this);
        newSound.Play();

        //Starts the couroutine to finish the explosion
        StartCoroutine(WarnAndExplode());
    }

    //Waits the given amount of time, then plays the explosion sound
    IEnumerator WarnAndExplode()
    {
        //Marks this explosion as in progress
        hasExploded = false;

        //
        Renderer rend = GetComponent<MeshRenderer>();

        rend.material.color = Color.cyan;

        //Waits a given amount of time before exploding
        yield return new WaitForSeconds(timeBeforeExplosion);

        //Plays the explosion sound, which may make enemies investigate
        newSound = new SoundClass(source, sound.clip, sound.range, this.transform.position);
        newSound.Play();

        //Plays the explosion particle system
        explosion.Play();

        //Marks the explosion as completed
        hasExploded = true;

        rend.material.color = Color.red;
    }
}