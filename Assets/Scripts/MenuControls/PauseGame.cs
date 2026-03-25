using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public static PauseGame Instance;
    public bool isPaused;
    public GameObject GameOverMenu;
    public GameObject GameOverContinue;
    public GameObject backgroundAudioSlider;
    public GameObject sfxAudioSlider;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        isPaused = false;
        backgroundAudioSlider.SetActive(false);
        sfxAudioSlider.SetActive(false);
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
    public void Stop()
    {
        Time.timeScale = 0;
        isPaused = true;
        GameOverMenu.SetActive(true);
        GameOverContinue.SetActive(true);
        backgroundAudioSlider.SetActive(true);
        sfxAudioSlider.SetActive(true);
    }
    public void ContinueGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        GameOverMenu.SetActive(false);
        GameOverContinue.SetActive(false);
        backgroundAudioSlider.SetActive(false);
        sfxAudioSlider.SetActive(false);
    }
}
