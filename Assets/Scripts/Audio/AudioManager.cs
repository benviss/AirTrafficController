using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;

    private Sound _currentMusic;

    private List<string> _transmissionSounds = new List<string>();
    private List<string> _spawnSounds = new List<string>();

    private void Awake()
    {
        foreach (var s in Sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;

            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;

            switch (s.Type)
            {
                case Sound.SoundType.Transmission:
                    _transmissionSounds.Add(s.Name);
                    break;

                case Sound.SoundType.Spawn:
                    _spawnSounds.Add(s.Name);
                    break;
            }
        }
    }

    public void PlayTransmissionSound()
    {
        PlayRandom(_transmissionSounds);
    }

    public void PlaySpawnSound()
    {
        PlayRandom(_spawnSounds);
    }

    private void PlayRandom(List<string> sounds)
    {
        var soundName = sounds[Random.Range(0, sounds.Count)];
        Play(soundName);
    }

    public void Play(string name)
    {
        var sound = Array.Find(Sounds, s => s.Name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Sound with name \"{name}\" was not found");
            return;
        }

        if (sound.Type == Sound.SoundType.Music)
        {
            if (_currentMusic != null)
                _currentMusic.Source.Stop();

            _currentMusic = sound;
        }

        sound.Source.Play();
    }

    public void PauseMusic()
    {
        if (_currentMusic != null)
        {
            _currentMusic.Source.Pause();
        }
    }

    public void UnpauseMusic()
    {
        if (_currentMusic != null)
        {
            _currentMusic.Source.Pause();
        }
    }
}

