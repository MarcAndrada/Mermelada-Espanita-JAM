using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _soundMInstance;

    [SerializeField] private AudioSource  _effectsSource;
    [SerializeField] AudioMixerGroup audioMixer;

    public Sounds[] sounds;

    private void Awake()
    {
        PlayerPrefs.SetFloat("volume", 0.5f);

        if (_soundMInstance == null)
        {
            _soundMInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach(Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audio;

            s.source.outputAudioMixerGroup = audioMixer;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Start()
    {
        SoundManager._soundMInstance.Play("backgroundMusic");
    }

    public void Update()
    {
        /*foreach (Sounds s in sounds)
        {
            s.source.volume = PlayerPrefs.GetFloat("volume");
        }*/
    }

    public void Play(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound not found");
            return;
        }
        s.source.Play();
    }

    public void ChangeVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void Stop(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found");
            return;
        }
        s.source.Stop();
    }
}
