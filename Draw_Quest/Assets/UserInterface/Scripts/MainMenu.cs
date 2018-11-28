using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Slider MusicVolumeSlider;
    [SerializeField] private AudioMixer MusicMixer;
    [Space]
    [SerializeField] private Slider SoundVolumeSlider;
    [SerializeField] private AudioMixer SoundMixer;
    [Space]

    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private GameObject QuitPanel;

    private void Awake()
    {
        if (OptionsPanel.activeSelf == true)
        {
            OptionsPanel.SetActive(false);
        }

        if (QuitPanel.activeSelf == true)
        {
            QuitPanel.SetActive(false);
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (PlayerPrefs.HasKey("SoundVolume"))
        {
            SoundVolumeSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        }
    }

    private void Start()
    {
        SetMusicVolume(MusicVolumeSlider.value);
        SetSoundVolume(SoundVolumeSlider.value);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("DrawingScene");
    }

    public void OptionsButton()
    {
        OptionsPanel.SetActive(true);
    }

    public void QuitButton()
    {
        QuitPanel.SetActive(true);
    }

    //--------------------------------------------------------OptionsPanel
    public void OptionsPanel_Apply()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolumeSlider.value);
        PlayerPrefs.SetFloat("SoundVolume", SoundVolumeSlider.value);
        OptionsPanel.SetActive(false);
    }
    //
    //-----------------------------------------------------------QuitPanel
    public void QuitPanel_Confirm()
    {
        Application.Quit();
    }
    public void QuitPanel_Cancel()
    {
        QuitPanel.SetActive(false);
    }
    //
    //-------------------------------------------------------------Options
    private float ConvertVolume(float volume)
    {
        switch (System.Convert.ToInt32(volume))
        {
            case 0: return -80.0f;
            case 1: return -30.0f;
            case 2: return -20.0f; 
            case 3: return -15.0f; 
            case 4: return -10.0f;
            case 5: return -9.0f; 
            case 6: return -7.0f; 
            case 7: return -5.0f; 
            case 8: return -3.0f; 
            case 9: return -2.0f; 
            case 10: return 0.0f;
            default: return 0.0f;
        }
    }

    public void SetMusicVolume(float volume)
    {
        MusicMixer.SetFloat("volume", ConvertVolume(volume));
    }
    public void SetSoundVolume(float volume)
    {
        SoundMixer.SetFloat("volume", ConvertVolume(volume));
    }
    //--------------------------------------------------------------------
}
