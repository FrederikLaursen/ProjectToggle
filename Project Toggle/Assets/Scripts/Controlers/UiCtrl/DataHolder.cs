using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour {

    private static DataHolder instance = null;

    [SerializeField]
    private string seed = null;
    [SerializeField]
    private string playthrough = null;
    [SerializeField]
    private string name = null;
    [SerializeField]
    private int score;
    private string id;

    private bool offline;

    public static DataHolder Instance
    {
        get
        {
            return instance;
        }
    }
    public string Seed
    {
        get
        {
            return seed;
        }

        set
        {
            seed = value;
        }
    }
    public string Playthrough
    {
        get
        {
            return playthrough;
        }

        set
        {
            playthrough = value;
        }
    }
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }
    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }
    public string ID
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public bool Offline
    {
        get
        {
            return offline;
        }

        set
        {
            offline = value;
        }
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
