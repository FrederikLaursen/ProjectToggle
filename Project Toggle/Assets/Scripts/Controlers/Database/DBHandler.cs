using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DBHandler : MonoBehaviour
{
    private static DBHandler instance = null;

    string dburl = "https://www.projecttoggle.dk/";
    string currentUsername = "";
    string currentUserId = "";
    List<Highscore> highscores = new List<Highscore>(); // This list contains the highscores

    void Start()
    {
    }

    public static DBHandler Instance
    {
        get
        {
            return instance;
        }
    }

    public List<Highscore> Highscores
    {
        get
        {
            return highscores;
        }

        set
        {
            highscores = value;
        }
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region onClick
    public void GetScoresOnLoad()
    {
        StartCoroutine(GetScores());
    }

    //public List<Highscore> GetHighscoreList()
    //{
    //    return Highscores;
    //}

    #endregion

    #region DB Methods
    IEnumerator GetScores()
    {
        WWW scoreGet = new WWW(dburl + "display.php");
        yield return scoreGet;

        Highscores.Clear();

        if (scoreGet.error != null)
            Debug.Log("An error occured: " + scoreGet.error);
        else
        {
            Debug.Log(scoreGet.text);
            foreach (string jsonObj in scoreGet.text.Split('}'))
            {
                if (jsonObj != "" && jsonObj != null)
                {
                    Highscore score = JsonUtility.FromJson<Highscore>(jsonObj + "}");
                    StartCoroutine(GetNameById(score));
                }
            }
         }
    }

    IEnumerator GetNameById(Highscore score)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", score.playerId);

        WWW nameGet = new WWW(dburl + "getuserbyid.php", form);
        yield return nameGet;

        Debug.Log(nameGet.url);
        if (nameGet.error != null)
            Debug.Log("An error occured: " + nameGet.error);
        else
        {
            score.playerName = nameGet.text;
            Debug.Log(score.playerId + " - " + score.playerName);
        }
        Highscores.Add(score);
        
        Highscores = Highscores.OrderByDescending(s => s.score).ToList();
        MenuLeaderboardCtrl.Instance.CreateLeaderboard();

    }

   public IEnumerator ResetPw(string username, Text feedback)
    {
        WWWForm form = new WWWForm();

        form.AddField("username", username);

        WWW webRequest = new WWW(dburl + "resetpassword.php", form);
        yield return webRequest;

        feedback.gameObject.SetActive(true);
    }

    public IEnumerator SignUp(string username, string password, string email, Text feedback)
    {
        WWWForm form = new WWWForm();

        form.AddField("id", Guid.NewGuid().ToString());
        form.AddField("name", username);
        form.AddField("password", password);
        form.AddField("email", email);
        Debug.Log(form.data.ToString());
        WWW webRequest = new WWW(dburl + "adduser.php", form);
        yield return webRequest;

        if (!string.IsNullOrEmpty(webRequest.error))
        {
            Debug.Log("Server is not responding" + webRequest.error);
        }
        else if (string.IsNullOrEmpty(webRequest.error))
        {
            if (webRequest.responseHeaders.Count > 0 && webRequest.responseHeaders.ContainsKey("USERCREATION"))
            {
                Debug.Log(webRequest.responseHeaders["USERCREATION"]);

                if (webRequest.responseHeaders["USERCREATION"] == "true")
                {
                    Debug.Log("Creation succesfull" + webRequest);
                    feedback.gameObject.SetActive(true);
                    feedback.text = "Creation succesfull.";
                }
                else
                {
                    Debug.Log("User exists" + webRequest);
                    feedback.gameObject.SetActive(true);
                    feedback.text = "Creation failed. User already exists.";
                }
            }
        }
    }

    public IEnumerator Login(string username, string password, Text errorText)
    {
        WWWForm form = new WWWForm();

        form.AddField("username", username);
        form.AddField("pw", password);

        WWW webRequest = new WWW(dburl + "verifylogin.php", form);
        yield return webRequest;
        
        if (!string.IsNullOrEmpty(webRequest.error))
        {
            Debug.Log(webRequest.error);
            errorText.text += webRequest.error;
        }
        else if (string.IsNullOrEmpty(webRequest.error))
        {
            if (webRequest.responseHeaders.Count > 0 && webRequest.responseHeaders.ContainsKey("LOGINSUCCEED"))
            {
                Debug.Log(webRequest.responseHeaders["LOGINSUCCEED"]);

                if (webRequest.responseHeaders["LOGINSUCCEED"] == "true")
                {
                    DataHolder.Instance.ID = webRequest.responseHeaders["PLAYERID"];
                    DataHolder.Instance.Name = webRequest.responseHeaders["PLAYERNAME"];
                    DataHolder.Instance.Offline = false;
                    SceneManager.LoadScene("menuscene");
                }
                else
                {
                    errorText.gameObject.SetActive(true);
                }
            }
        }
    }

    public IEnumerator SetScore(string ID, int score, string mapSeed, string playthrough)
    {
        WWWForm form = new WWWForm();

        form.AddField("playerID", ID);
        form.AddField("score", score);

        form.AddField("mapSeed", mapSeed);
        form.AddField("playthrough", playthrough);

        WWW webRequest = new WWW(dburl + "addscore.php", form);
        yield return webRequest;
    }
}

#endregion



