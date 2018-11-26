using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer MusicMixer;
    [SerializeField] private AudioMixer SoundMixer;

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
    public void SetVolume(float volume)
    {
        //MusicMixer.
        MusicMixer.SetFloat("volume", volume);
        //AudioListener.volume = volume;
    }
    //--------------------------------------------------------------------
}
