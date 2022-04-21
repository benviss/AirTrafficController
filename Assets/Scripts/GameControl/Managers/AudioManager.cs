using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource spawnSource;
    public AudioSource transmissionSource;

    public AudioClip[] spawnAudioSources;
    public AudioClip[] transmissionAudioSources;


    public void PlayTransmissionSound()
    {
        transmissionSource.clip = transmissionAudioSources[Random.Range(0, transmissionAudioSources.Length)];
        transmissionSource.Play();
    }


    public void PlayTurnSound()
    {
        spawnSource.clip = spawnAudioSources[Random.Range(0, spawnAudioSources.Length)];
        spawnSource.Play();
    }
}
