using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSample[] sounds;
    private void Awake()
    {
        foreach(AudioSample s in sounds)
        {
            s.source=gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    public void Play(string name)
    {
        AudioSample s=Array.Find(sounds, sound => sound.name == name);
        if(s!=null)
            s.source.Play();
    }
    public void Stop(string name)
    {
        AudioSample s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Stop();
    }
    public void StopAll()
    {
        foreach(AudioSource s in GetComponents<AudioSource>())
        {
            s.Stop();
        }
    }
    public void PlayBackground()
    {
        int rnd = UnityEngine.Random.Range(0, 2);
        Play("bg" + rnd);
    }
}
