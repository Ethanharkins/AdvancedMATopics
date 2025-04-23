using UnityEngine;

public class WarningSoundClass : SoundClass
{
    public ExplodeOnClickScript explosionSource;

    //Sets the explosion source so we can pass it along
    public WarningSoundClass(AudioSource source, AudioClip clip, float range, Vector3 position, ExplodeOnClickScript explosionSource) : base(source, clip, range, position)
    {
        this.explosionSource = explosionSource;
    }

    /// <summary>
    /// Notifies all EnemyExplosionAwareness scripts that a bomb will go off
    /// </summary>
    public virtual void RangeNotify()
    {
        Collider[] listeners = Physics.OverlapSphere(this.position, this.range);

        //For each collider in range
        foreach (var listener in listeners)
        {
            //If the game object with the collider has awareness of explosions, 
            if (listener.TryGetComponent<EnemyExplosionAwareness>(out var enemyExplosionAwareness))
            {
                //Tell it to check if they could hear this sound
                enemyExplosionAwareness.CheckIfHeard(this);
            }
        }
    }
}