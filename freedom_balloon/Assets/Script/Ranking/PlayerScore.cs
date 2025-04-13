using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerScore
{
    public string nickname;
    public int score;

    public PlayerScore(string name, int score)
    {
        this.nickname = name;
        this.score = score;
    }
}