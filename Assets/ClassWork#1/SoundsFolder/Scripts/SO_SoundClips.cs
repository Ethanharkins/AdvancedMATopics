using UnityEngine;

[CreateAssetMenu(fileName = "SO_SoundClip", menuName = "Scriptable Objects/SO_SoundClip")]
public class SO_SoundClip : ScriptableObject
{
    public AudioClip clip;
    public float range;
}