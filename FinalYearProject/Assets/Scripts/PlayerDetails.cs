using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerDetails
{
    public string username;
    public int level;
    public int score;
    
    public PlayerDetails(string username, int level, int score)
    {
        this.username = username;
        this.level = level;
        this.score = score;
    }
}
