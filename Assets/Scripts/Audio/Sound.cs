using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;

    public enum SoundType
    {
        Music,
        Transmission,
        Spawn,
        Crash
    }

    public SoundType Type;

    public AudioClip Clip;

    [Range(0f, 1f)]
    public float Volume;
    [Range(.1f, 3f)]
    public float Pitch;

    public bool Loop;

    [HideInInspector]
    public AudioSource Source;
}

