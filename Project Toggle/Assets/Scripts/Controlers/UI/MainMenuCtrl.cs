using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCtrl : MonoBehaviour {

    [SerializeField]
    private Text txtRef;
    [SerializeField]
    private Button leaderboard;
    [SerializeField]
    private Sprite offlineSprite, onlineSprite;
    // Use this for initialization
    void Start ()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        if (!string.IsNullOrEmpty(DataHolder.Instance.Name))
        {
            txtRef.text = "Hello " + DataHolder.Instance.Name;
        }
        else
        {
            txtRef.text = "Hello guest";
        }

        if (!DataHolder.Instance.Offline)
        {
            leaderboard.interactable = true;
            leaderboard.image.sprite = onlineSprite;
        }
        else
        {
            leaderboard.interactable = false;
            leaderboard.image.sprite = offlineSprite;
        }
    }

    public void PlayOnClick()
    {
        SceneManager.LoadScene("gameScene");

    }

    public void LeaderboardOnClick()
    {
        if (!DataHolder.Instance.Offline)
        {
            SceneManager.LoadScene("LeaderboardScene");
        }
    }

    public void SettingsOnClick()
    {
        SceneManager.LoadScene("SettingsScene");
    }
}
