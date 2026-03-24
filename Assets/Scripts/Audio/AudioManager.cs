using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private List<AbilitySoundEntry> sounds;
    [SerializeField] private AudioClip HurtClip;
    [SerializeField] private AudioClip DeathClip;
    [SerializeField] private AudioSource ambientsource;
    [SerializeField] private AudioSource sfxsource;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider backgroundAudioSlider;
    [SerializeField] private Slider sfxAudioSlider;
    private Dictionary<AbilitySoundType, AudioClip> soundMap;
    private void Awake()
    {
        Instance = this;
        soundMap = new Dictionary<AbilitySoundType, AudioClip>();
        foreach (var entry in sounds)
        {
            soundMap[entry.type] = entry.clip;
        }
        ambientsource.loop = true;
        ambientsource.outputAudioMixerGroup = mixer.FindMatchingGroups("Background")[0];
        ambientsource.Play();
        backgroundAudioSlider.onValueChanged.AddListener(SetBackgroundVolume);
        sfxsource.outputAudioMixerGroup = mixer.FindMatchingGroups("Sfx")[0];
        sfxAudioSlider.onValueChanged.AddListener(SetSFXVolume);
        backgroundAudioSlider.value = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
        sfxAudioSlider.value = PlayerPrefs.GetFloat("SfxVolume", 1f);
    }
    public AudioClip GetClip(AbilitySoundType type)
    {
        return soundMap.TryGetValue(type, out var clip) ? clip : null;
    }
    public void PlaySound(AudioClip clip, Vector3 position)
    {
        Debug.Log("Ah");
        if (clip != null)
        {
            Debug.Log(clip);
            GameObject obj = new("Audio");
            obj.transform.position = position;

            var source = obj.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialBlend = 1f;
            source.minDistance = 20f;
            source.maxDistance = 50f;
            source.outputAudioMixerGroup = mixer.FindMatchingGroups("Sfx")[0];

            source.Play();
            Destroy(obj, clip.length);
        }
    }
    public void PlayerDies()
    {
        sfxsource.PlayOneShot(DeathClip);
    }
    public void PlayerHurt()
    {
        sfxsource.PlayOneShot(HurtClip);
    }
    public void SetSFXVolume(float slidervalue)
    {
        mixer.SetFloat("SfxVolume", Mathf.Log10(slidervalue) * 20);
        PlayerPrefs.SetFloat("SfxVolume", slidervalue);
    }
    public void SetBackgroundVolume(float slidervalue)
    {
        mixer.SetFloat("BackgroundVolume", Mathf.Log10(slidervalue) * 20);
        PlayerPrefs.SetFloat("BackgroundVolume", slidervalue);
    }
}
[System.Serializable]
public class AbilitySoundEntry
{
    public AbilitySoundType type;
    public AudioClip clip;
}