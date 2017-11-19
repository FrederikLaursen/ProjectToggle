using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighscoreBtnCtrl : MonoBehaviour {

    public string seed;
    public string playthrough;
    public string name;
    public int score;
    
	
    public void OnClick()
    {
        DataHolder.Instance.Seed = seed;
        DataHolder.Instance.Playthrough = playthrough;
        DataHolder.Instance.Name = name;
        DataHolder.Instance.Score = score;

        SceneManager.LoadScene("gameScene"); 
    }
}