using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    public Slider volSlider;
    public Dropdown resolutionDropdown;
    public Dropdown graphicsDropdown;

    Resolution[] resolutions;

    void Start ()
    {
        // ustaw na poczatku wartosci 

        float volume = PlayerPrefs.GetFloat("volume");
        int qualityIndex = PlayerPrefs.GetInt("quality");
        int width_scr = PlayerPrefs.GetInt("width");
        int height_scr = PlayerPrefs.GetInt("height");
        Screen.SetResolution(width_scr, height_scr, Screen.fullScreen);

        QualitySettings.SetQualityLevel(qualityIndex);
        graphicsDropdown.value = qualityIndex;

        audioMixer.SetFloat("volume", volume);
        volSlider.value = volume;

        // ------------------------

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int CurrentresolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                option = resolutions[i].width + " x " + resolutions[i].height;
                CurrentresolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = CurrentresolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void setFullscreen (bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        if (Screen.fullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    public void saveButton()
    {
        PlayerPrefs.SetFloat("volume", volSlider.value);
        PlayerPrefs.SetInt("quality", QualitySettings.GetQualityLevel());
        PlayerPrefs.SetInt("width", Screen.currentResolution.width);
        PlayerPrefs.SetInt("height", Screen.currentResolution.height);
    }
}
