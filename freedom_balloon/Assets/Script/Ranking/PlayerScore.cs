using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 점수 데이터를 저장하는 클래스
[System.Serializable] // JSON 저장을 위해 직렬화 가능하게 설정
public class PlayerScore
{
    public string nickname;  // 플레이어 닉네임
    public int score;        // 획득한 점수

    public PlayerScore(string name, int score)
    {
        this.nickname = name;
        this.score = score;
    }
}