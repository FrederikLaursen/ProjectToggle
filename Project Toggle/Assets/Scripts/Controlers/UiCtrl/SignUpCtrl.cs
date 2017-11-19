using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class SignUpCtrl : MonoBehaviour
{
    [SerializeField]
    Text feedBack;

    public void SignUpOnClick()
    {
        Text usrname = GameObject.Find("SignUpUsrText").GetComponent<Text>();
        Text password = GameObject.Find("SignUpPasswordText").GetComponent<Text>();
        Text email = GameObject.Find("SignUpEmailText").GetComponent<Text>();
        DBHandler.Instance.StartCoroutine(DBHandler.Instance.SignUp(usrname.text.ToString(), password.text.ToString(), email.text.ToString(), feedBack));
    }

    public void SignUpBck()
    {
        SceneManager.LoadScene("Login");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Login");
        }
    }
}


