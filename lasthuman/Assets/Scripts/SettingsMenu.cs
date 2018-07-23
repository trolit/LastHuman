using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    // 
    public GameObject settingMenu;
    public GameObject pauseMenu;

    // dropdowns here
    public Dropdown resolutionDropdown;
    public Dropdown graphicsDropdown;
    public Dropdown antialiasingDropdown;
    public Dropdown textureQualityDropdown;

    // toggles here
    public Toggle vsynctoggle;

    // arrays here
    Resolution[] resolutions;

    void Awake()
    {
        int antialiasing = PlayerPrefs.GetInt("ant_a");
        // load antialiasing 
        QualitySettings.antiAliasing = antialiasing;
        SetAntiAliasingDropdown(antialiasing);
    }

    void Start ()
    {
        // fix?
        useGUILayout = false;

        // loading parameters...
        int qualityIndex = PlayerPrefs.GetInt("quality");
        int width_scr = PlayerPrefs.GetInt("width");
        int height_scr = PlayerPrefs.GetInt("height");
        int texture_quality = PlayerPrefs.GetInt("text_q");

        // load screen resolution
        Screen.SetResolution(width_scr, height_scr, Screen.fullScreen);

        // load quality level
        QualitySettings.SetQualityLevel(qualityIndex);
        graphicsDropdown.value = qualityIndex;

        // load texture quality
        QualitySettings.masterTextureLimit = texture_quality;
        textureQualityDropdown.value = texture_quality;

        // ------------------------
        // Setting up resolution options
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
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        /*
        problem z tym fragmentem jest taki, ze jak wybieramy opcje grafiki np. 
        medium i potem wybierzemy high to nie znika symbol "V" obok...
        */

        // komentarz(6 lipca 18) - o, dziala teraz normalnie :D
        antialiasingDropdown.value = (int)QualitySettings.antiAliasing - 1;
        antialiasingDropdown.RefreshShownValue();
        textureQualityDropdown.value = (int)QualitySettings.masterTextureLimit;
        textureQualityDropdown.RefreshShownValue();
        graphicsDropdown.RefreshShownValue();
        if(QualitySettings.vSyncCount > 0)
        {
            vsynctoggle.isOn = true;
        }
        else if(QualitySettings.vSyncCount == 0)
        {
            vsynctoggle.isOn = false;
        }
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
    public void setVsync (bool vsync)
    {
        if (vsync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }
    public void setAntialiasing (int setaa)
    {
        if (setaa == 0)
        {
            QualitySettings.antiAliasing = 0;
        }
        else if(setaa == 1)
        {
            QualitySettings.antiAliasing = 2;
        }
        else if (setaa == 2)
        {
            QualitySettings.antiAliasing = 4;
        }
        else if (setaa == 3)
        {
            QualitySettings.antiAliasing = 8;
        }
    }
    public void setTexQuality (int textq)
    {
        if(textq == 0)
        {
            QualitySettings.masterTextureLimit = 0;
        }
        else if(textq == 1)
        {
            QualitySettings.masterTextureLimit = 1;
        }
        else if (textq == 2)
        {
            QualitySettings.masterTextureLimit = 2;
        }
        else if (textq == 3)
        {
            QualitySettings.masterTextureLimit = 3;
        }
    }
    void SetAntiAliasingDropdown(int antialiasing)
    {
        if (antialiasing == 0)
        {
            antialiasingDropdown.value = (int)0;
        }
        else if (antialiasing == 2)
        {
            antialiasingDropdown.value = (int)1;
        }
        else if (antialiasing == 4)
        {
            antialiasingDropdown.value = (int)2;
        }
        else if (antialiasing == 8)
        {
            antialiasingDropdown.value = (int)3;
        }
    }

    public void saveButton()
    {
        PlayerPrefs.SetInt("quality", QualitySettings.GetQualityLevel());
        PlayerPrefs.SetInt("width", Screen.currentResolution.width);
        PlayerPrefs.SetInt("height", Screen.currentResolution.height);
        PlayerPrefs.SetInt("ant_a", QualitySettings.antiAliasing);
        PlayerPrefs.SetInt("text_q", QualitySettings.masterTextureLimit);

        // will not probably save vsync parameter
        // PlayerPrefs.SetInt("vsync", QualitySettings.vSyncCount);
    }
}
