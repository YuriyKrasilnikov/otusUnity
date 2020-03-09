using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class InPauseMenu : MonoBehaviour
{
    [Serializable]
    public class SoundVolumeSliderSettingsData
    {
        public string Title;
        public Slider VolumeSlider;
        public TextMeshProUGUI VolumeText;
        public string VolumeParamNameInMixer;
        public static AudioMixer Mixer { get; private set; } = null;
        //private bool _init = false;

        public static void InitMixer(AudioMixer mixer)
        {
            Mixer = mixer;
        }

        private void OnSliderValueChanged(float param)
        {
            Mixer.SetFloat(VolumeParamNameInMixer, param);
            VolumeText.text = $"{Title}: {(param + 80):N0}";
        }

        public void Init()
        {

            float currentVolume;
            Mixer.GetFloat(VolumeParamNameInMixer, out currentVolume);
            VolumeText.text = $"{Title}: {(currentVolume + 80):N0}";
            VolumeSlider.value = currentVolume;

            VolumeSlider.onValueChanged.AddListener(OnSliderValueChanged);

        }
    }

    public GameObject PauseScreen;
    public Button ResumeButton;
    public Button SettingButton;
    public Button RestartButton;
    public Button MenuButton;
    public GameObject WinText;
    public GameObject LostText;

    public SoundVolumeSliderSettingsData[] SoundVolumeSettings;

    public GameObject SettingScreen;
    public Button BackButton;

    public AudioMixer Mixer;

    private void Awake()
    {
        if (!SoundVolumeSliderSettingsData.Mixer)
            SoundVolumeSliderSettingsData.InitMixer(Mixer);

        foreach (var soundVolumeItem in SoundVolumeSettings)
        {
            soundVolumeItem.Init();
        }
    }
}
