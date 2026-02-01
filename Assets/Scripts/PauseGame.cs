using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    bool isPaused;
    public GameObject GameOverMenu;
    public GameObject GameOverContinue;
    public GameObject AudioSlider;
    public InputActionAsset InputActions;
    private InputAction pause;
    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }
    private void Awake()
    {
        pause = InputSystem.actions.FindAction("Pause");
    }
    void Start()
    {
        isPaused = false;
        AudioSlider.SetActive(false);
        GameOverMenu.SetActive(false);
        GameOverContinue.SetActive(false);
    }
    void Update()
    {
        if(pause.WasPressedThisFrame())
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
