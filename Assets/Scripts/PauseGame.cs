using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    bool isPaused;
    public GameObject GameOverMenu;
    public GameObject GameOverContinue;
    public GameObject AudioSlider;
    void Start()
    {
        isPaused = false;
        AudioSlider.SetActive(false);
        GameOverMenu.SetActive(false);
        GameOverContinue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown (KeyCode.Escape))
        {
            if (!isPaused)
            {
                stop();
            }
            else
            {
                continueGame();
            }
        }
    }
    void stop()
    {
        Time.timeScale = 0;
        isPaused = true;
        GameOverMenu.SetActive(true);
        GameOverContinue.SetActive(true);
        AudioSlider.SetActive(true);
    }
    public void continueGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        GameOverMenu.SetActive(false);
        GameOverContinue.SetActive(false);
        AudioSlider.SetActive(false);
    }
}
