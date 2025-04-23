using UnityEngine;

public class SoundClass
{
    //The source that plays this sound
    public AudioSource source;
    //The clip that we're playing
    public AudioClip clip;
    //The range of the sound
    public float range;
    //The position of the sound
    public Vector3 position;

    //A constructor for this class so we can pass information to the object
    public SoundClass(AudioSource source, AudioClip clip, float range, Vector3 position)
    {
        this.source = source;
        this.clip = clip;
        this.range = range;
        this.position = position;
    }

    /// <summary>
    /// Play the sound of this SoundClass object
    /// </summary>
    public void Play()
    {
        this.source.clip = this.clip;
        this.source.transform.position = this.position;
        this.source.Play();

        RangeNotify();
    }

    /// <summary>
    /// Gets all the EnemyHearingScript instances in range and notifies them that this sound has been played.
    /// </summary>
    private void RangeNotify()
    {
        Collider[] listeners = Physics.OverlapSphere(this.position, this.range);

        //For each collider in range
        foreach (var listener in listeners)
        {
            //If the game object with the collider has an EnemyHearingScript, 
            if (listener.TryGetComponent<EnemyHearingScript>(out var enemyHearingScript))
            {
                //Tell it to check if they could hear this sound
                enemyHearingScript.CheckIfHeard(this);
            }
        }
    }
}