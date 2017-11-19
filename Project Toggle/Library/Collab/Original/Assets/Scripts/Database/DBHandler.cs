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

    string dburl = "https://138.68.67.66/";
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
        DontDestroyOnLoad(this.gameObject);
    }

    #region onClick
    public void GetScoresOnLoad()
    {
        StartCoroutine(GetScores());
    }

    //public void ResetPasswordOnClick()
    //{
    //    Text temp = GameObject.Find("resetpwtext").GetComponent<Text>();
    //    Debug.Log(temp.text.ToString());
    //    StartCoroutine(ResetPw(temp.text.ToString()));
    //}


    //public void SignUpOnClick()
    //{
    //    Text usrname = GameObject.Find("SignUpUsrText").GetComponent<Text>();
    //    Text password = GameObject.Find("SignUpPasswordText").GetComponent<Text>();
    //    Text email = GameObject.Find("SignUpEmailText").GetComponent<Text>();
    //    StartCoroutine(SignUp(usrname.text.ToString(), password.text.ToString(), email.text.ToString()));
    //}

    //public void LoginOnClick()
    //{
    //    Text usrname = GameObject.Find("LoginUpUsrText").GetComponent<Text>();
    //    Text password = GameObject.Find("LoginPasswordText").GetComponent<Text>();
    //    StartCoroutine(Login(usrname.text.ToString(), password.text.ToString()));
    //}

    public List<Highscore> GetHighscoreList()
    {
        return Highscores;
    }

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

   public IEnumerator ResetPw(string username)
    {

        WWWForm form = new WWWForm();

        form.AddField("username", username);

        WWW webRequest = new WWW(dburl + "resetpassword.php", form);
        yield return webRequest;
    }

    public IEnumerator SignUp(string username, string password, string email)
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
                }
                else
                {
                    Debug.Log("User exists" + webRequest);
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

                    SceneManager.LoadScene("menuscene");
                }
                else
                {
                    errorText.text += " 216";
                    errorText.gameObject.SetActive(true);
                    errorText.text = "Wrong password or username";
                    Debug.Log("Wrong password or username" + webRequest);
                }
            }
        }
    }

    public IEnumerator SetScore(string ID, int score, string mapSeed, string playthrough)
    {

        //TODO Tjek om din score er højere end den vi har opbevaret. Serverside eller client tbd
        WWWForm form = new WWWForm();

        form.AddField("playerID", ID);
        form.AddField("score", score);

        form.AddField("mapSeed", mapSeed);
        form.AddField("playthrough", playthrough);

        WWW webRequest = new WWW(dburl + "addscore", form);
        yield return webRequest;
    }
}

#endregion



