using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{
    private GameObject pause_panel;

    private bool isPaused = false;

    void Awake()
    {
        pause_panel = gameObject;

        if (pause_panel.activeSelf == true)
        {
            pause_panel.SetActive(false);
        }
    }

    public void Pause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;

            pause_panel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;

            pause_panel.SetActive(false);
        }

        isPaused = !isPaused;
    }

    public void Restart()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AppQuit()
    {
        Application.Quit();
    }
}
