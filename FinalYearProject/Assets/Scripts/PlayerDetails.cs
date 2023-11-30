using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerDetails
{
    public int items;
    public int level;
    public int score;
    public float[] position;
    
    public PlayerDetails(GameController player)
    {
        items = player.items;
        level = player.level;
        score = player.score;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

    }
}
