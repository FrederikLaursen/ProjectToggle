using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] // Allow serialization to JSON
public class Highscore
{
    public int score;
    public string playerId;
    public string playerName;
    public string mapSeed;
    public string playThrough;
}
