using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXManager: SingletonMonoBehaviour<SFXManager>
{
    public AudioSource[] audioSources;
    private System.Random rand = new System.Random();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if(audioSource)
            {
                audioSource.Stop();
            }
        }
    }

    public AudioSource Play(AudioClip[] clips, Vector3 position, bool onLoop=false)
    {
        AudioSource freeAudioSource = FindFreeAudioSource();

        freeAudioSource.transform.position = position;

        var clip = clips[rand.Next(clips.Length)];
        freeAudioSource.clip = clip;

        freeAudioSource.loop = onLoop;
        freeAudioSource.Play();

        return freeAudioSource;
    }

    private AudioSource FindFreeAudioSource()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (!audioSource.isPlaying)
                return audioSource;
        }

        Debug.LogError($"Overload of sounds");
        return null;
    }
}
