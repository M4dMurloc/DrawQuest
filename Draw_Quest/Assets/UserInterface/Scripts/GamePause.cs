using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{
    private bool isPaused = false;

    public void Pause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;

            gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;

            gameObject.SetActive(false);
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
