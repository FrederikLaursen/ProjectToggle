using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetrievePasswordCtrl : MonoBehaviour {
    [SerializeField]
    Text feedback;
    public void ResetPw()
    {
        Text username = GameObject.Find("resetpwtext").GetComponent<Text>();
        if (username.text != "")
        {
            DBHandler.Instance.StartCoroutine(DBHandler.Instance.ResetPw(username.text.ToString(), feedback));
        }
    }

    public void ResetPwBck()
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

