using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLeaderboardCtrl : MonoBehaviour {

    private static MenuLeaderboardCtrl instance = null;

   

    [SerializeField]
    ScrollRect scrollRect;
    [SerializeField]
    GameObject highScoreBtn;
    [SerializeField]
    Transform contentTransform;

    List<Highscore> highscores = new List<Highscore>();

    public static MenuLeaderboardCtrl Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start ()
    {
        DBHandler.Instance.GetScoresOnLoad();
    }
    
    public void CreateLeaderboard()
    {
        // Remove all children
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        for (int i = 0; i < DBHandler.Instance.Highscores.Count; i++)
        {
            GameObject newHighscore = Instantiate(highScoreBtn);
            newHighscore.transform.FindChild("NameText").GetComponent<Text>().text = DBHandler.Instance.Highscores[i].playerName;
            newHighscore.transform.FindChild("ScoreText").GetComponent<Text>().text = DBHandler.Instance.Highscores[i].score.ToString();
            newHighscore.transform.FindChild("PlacementText").GetComponent<Text>().text = i +1 + ".";

            newHighscore.GetComponent<HighscoreBtnCtrl>().seed = DBHandler.Instance.Highscores[i].mapSeed;
            newHighscore.GetComponent<HighscoreBtnCtrl>().playthrough = DBHandler.Instance.Highscores[i].playThrough;
            newHighscore.GetComponent<HighscoreBtnCtrl>().name = DBHandler.Instance.Highscores[i].playerName;
            newHighscore.GetComponent<HighscoreBtnCtrl>().score = DBHandler.Instance.Highscores[i].score;

            newHighscore.transform.SetParent(transform, false);
        }
    }

    public void BackBtnOnClick()
    {
        SceneManager.LoadScene("MenuScene");
    }
}

