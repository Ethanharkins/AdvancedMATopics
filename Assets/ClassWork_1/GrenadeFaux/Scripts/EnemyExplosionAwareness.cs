using UnityEngine;

public class EnemyExplosionAwareness : EnemyHearingScript
{
    public new bool canHearThroughWalls
    {
        get
        {
            return false;
        }
    }
}