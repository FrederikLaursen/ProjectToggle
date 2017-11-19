using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginBtnCtrl : MonoBehaviour
{

    [SerializeField]
    Text errorText;

    public void LoginOnClick()
    {
        Text username = GameObject.Find("LoginUsrText").GetComponent<Text>();
        Text password = GameObject.Find("LoginPasswordText").GetComponent<Text>();
        DBHandler.Instance.StartCoroutine(DBHandler.Instance.Login(username.text.ToString(), password.text.ToString(), errorText));
    }

    public void SignUpScene()
    {
        SceneManager.LoadScene("SignUp");
    }

    public void RetrievePassword()
    {
        SceneManager.LoadScene("RetrievePassword");
    }

    public void PlayOffline()
    {
        DataHolder.Instance.Offline = true;
        SceneManager.LoadScene("menuscene");
    }
}