using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 10;
    private bool timerIsRunning;
    public float minutes;
    public float seconds;
    public TMP_Text timerText;
    public EndScreen endScreen;
    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
    }

    void DisplayTime(float timeToDisplay) {
        minutes = Mathf.FloorToInt(timeToDisplay / 60);
        seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if (seconds >= 10) {
            timerText.text = minutes.ToString() + ":" + seconds.ToString();
        }
        else {
            timerText.text = minutes.ToString() + ":0" + seconds.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else {
                timerIsRunning = false;
                timeRemaining = 0;
                DisplayTime(timeRemaining);
                endScreen.showScreen();
            }
        }
    }
}
