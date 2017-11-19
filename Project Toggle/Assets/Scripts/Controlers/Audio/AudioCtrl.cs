using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCtrl : MonoBehaviour {
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public static void playMusic(AudioSource source, AudioClip clip)
    {
        if (source != null && clip != null)
        {
            source.clip = clip;
            source.volume = PlayerPrefs.GetFloat("MusicSlider");
            source.Play();
            
        }
    }

    public static void playSFX(AudioSource source, AudioClip clip, float indVolume = 1)
    {
        if (source != null && clip != null)
        {
            source.clip = clip;
            source.volume = indVolume * PlayerPrefs.GetFloat("SfxSlider");
            source.Play();
        }
    }

    public static void vibrate()
    {
        if (PlayerPrefs.GetInt("VibrationToggle") == 1)
        {
            Handheld.Vibrate();
        }
    }
}
