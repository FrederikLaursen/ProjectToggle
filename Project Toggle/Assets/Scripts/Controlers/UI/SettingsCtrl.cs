using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsCtrl : MonoBehaviour {

    [SerializeField]
    public Toggle vibrationToggle;
    [SerializeField]
    public Slider sfxSlider;
    [SerializeField]
    public Slider musicSlider;
    // Use this for initialization
    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicSlider");
        sfxSlider.value = PlayerPrefs.GetFloat("SfxSlider");
        if (PlayerPrefs.GetInt("VibrationToggle") == 1)
        {
            vibrationToggle.isOn = true;

        }
        else if (PlayerPrefs.GetInt("VibrationToggle") == 0)
        {
            vibrationToggle.isOn = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene");
        }
    }

    public void VibrationToggleValueChanged()
    {
        if (vibrationToggle.isOn == true)
        {
            PlayerPrefs.SetInt("VibrationToggle", 1);
        }
        else if (vibrationToggle.isOn == false)
        {
            PlayerPrefs.SetInt("VibrationToggle", 0);
        }

    }

    public void MusicSliderValueChanged()
    {
        PlayerPrefs.SetFloat("MusicSlider", musicSlider.value);
    }

    public void SoundEffectSliderValueChanged()
    {
        PlayerPrefs.SetFloat("SfxSlider", sfxSlider.value);
    }

    public void BackBtnOnClick()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
