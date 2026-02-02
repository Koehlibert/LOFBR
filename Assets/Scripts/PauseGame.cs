using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public static PauseGame Instance;
    bool isPaused;
    public GameObject GameOverMenu;
    public GameObject GameOverContinue;
    public GameObject AudioSlider;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        isPaused = false;
        AudioSlider.SetActive(false);
        GameOverMenu.SetActive(false);
        GameOverContinue.SetActive(false);
    }
    public void TogglePause()
    {
        if (!isPaused)
            {
                Stop();
            }
            else
            {
                ContinueGame();
            }
    }
    void Stop()
    {
        Time.timeScale = 0;
        isPaused = true;
        GameOverMenu.SetActive(true);
        GameOverContinue.SetActive(true);
        AudioSlider.SetActive(true);
    }
    public void ContinueGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        GameOverMenu.SetActive(false);
        GameOverContinue.SetActive(false);
        AudioSlider.SetActive(false);
    }
}
