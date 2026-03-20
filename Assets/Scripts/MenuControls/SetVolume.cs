using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    private Slider audioSlider;
    private float volValue;
    void OnEnable()
    {
        audioSlider = GetComponent<Slider>();
        audioSlider.value = PlayerPrefs.GetFloat("Volume");
    }
    void OnDisable()
    {
        PlayerPrefs.SetFloat("Volume",audioSlider.value);
    }
    public void SetLevel(float slidervalue)
    {
        mixer.SetFloat("Vol", Mathf.Log10(slidervalue) * 20);
    }
}
