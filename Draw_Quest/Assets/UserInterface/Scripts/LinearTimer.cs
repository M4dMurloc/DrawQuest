using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinearTimer : MonoBehaviour
{
    [SerializeField] private float timerRange;
    [SerializeField] private GameObject timeLeftObject;

    private Image timerBar;
    private float timeLeft;

    void Start ()
    {
        timeLeftObject.SetActive(false);
        timerBar = GetComponent<Image>();
        timeLeft = timerRange;
    }
	
	void Update ()
    {
	    if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = (((int)(timeLeft * 100)) / 100f) / timerRange;
        }
        else
        {
            timeLeftObject.SetActive(true);
            Time.timeScale = 0;
        }
	}

    public void SetTime(int time)
    { 
        timerRange = time;
        timeLeft = time;
    }

    public void AddTime(int time)
    {
        timerRange += time;
        timeLeft += time;
    }
}
