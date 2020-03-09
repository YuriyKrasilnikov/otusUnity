using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BackgroundSoundControl : SingletonMonoBehaviour<BackgroundSoundControl>
{
    [System.Serializable]
    public class LevelSound
    {
        public string levelName;
        public List<AudioClip> clips;
    }

    public List<LevelSound> BgSoundList = new List<LevelSound>{
        new LevelSound{
            levelName = "default"
        }
    };

    private AudioSource _audioSource;
    private List<AudioClip> _levelClips;
    private System.Random rand = new System.Random();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);

        _audioSource = this.gameObject.GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        foreach (LevelSound sound in BgSoundList)
        {
            if (sound.levelName == scene.name)
            {
                _levelClips = sound.clips;
                break;
            }
            else if (sound.levelName == "default")
            {
                _levelClips = sound.clips;
            }
        }

        if (_audioSource)
        {
            _audioSource.Stop();
        }
    }

    void Update()
    {
        if (!_audioSource.isPlaying && _levelClips.Count>0)
        {
            _audioSource.clip = _levelClips[rand.Next(_levelClips.Count)];
            _audioSource.Play();
        }
    }
}
