using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

    public AudioMixer mastermixer;
    public AudioMixer sfxmixer;
    public AudioMixer soundtrmixer;

    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider soundtrackSlider;

    void Start()
    {
        // load Master Volume
        float volume = PlayerPrefs.GetFloat("volume");
        mastermixer.SetFloat("volume", volume);
        masterSlider.value = volume;

        // load SFX Volume
        float sfx = PlayerPrefs.GetFloat("sfxvol");
        sfxmixer.SetFloat("sfxvol", sfx);
        sfxSlider.value = sfx;

        // load soundtrack Volume
        float stvol = PlayerPrefs.GetFloat("soundtvol");
        soundtrmixer.SetFloat("soundtvol", stvol);
        soundtrackSlider.value = stvol;
    }

    public void SetMasterMixerLevel(float sliderValue)
    {
        mastermixer.SetFloat("volume", Mathf.Log(sliderValue) * 20);
    }

    public void SetSFXMixerLevel(float sliderValue)
    {
        sfxmixer.SetFloat("sfxvol", Mathf.Log(sliderValue) * 20);
    }

    public void SetSoundTrackMixerLevel(float sliderValue)
    {
        soundtrmixer.SetFloat("soundtvol", Mathf.Log(sliderValue) * 20);
    }

    public void SaveVolumes()
    {
        // save audio mixer sliders values
        PlayerPrefs.SetFloat("volume", masterSlider.value);
        PlayerPrefs.SetFloat("sfxvol", sfxSlider.value);
        PlayerPrefs.SetFloat("soundtvol", soundtrackSlider.value);
    }
}
