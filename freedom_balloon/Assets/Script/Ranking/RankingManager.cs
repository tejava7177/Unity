using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public static RankingManager instance;

    private const string RankingKey = "RankingData";
    public List<PlayerScore> rankings = new List<PlayerScore>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동에도 유지
            LoadRankings(); // 게임 시작 시 불러오기
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 🔽 랭킹 불러오기
    public void LoadRankings()
    {
        rankings.Clear();

        for (int i = 0; i < 5; i++)
        {
            string key = $"Rank_{i}";
            if (PlayerPrefs.HasKey(key))
            {
                string json = PlayerPrefs.GetString(key);
                PlayerScore data = JsonUtility.FromJson<PlayerScore>(json);
                rankings.Add(data);
            }
        }
    }

    // 🔼 새 점수 저장 시 랭킹 업데이트
    public void AddNewScore(string nickname, int score)
    {
        PlayerScore newScore = new PlayerScore(nickname, score);
        rankings.Add(newScore);

        // 점수 내림차순 정렬
        rankings.Sort((a, b) => b.score.CompareTo(a.score));

        // 상위 5개만 저장
        if (rankings.Count > 5)
            rankings.RemoveRange(5, rankings.Count - 5);

        // 저장
        for (int i = 0; i < rankings.Count; i++)
        {
            string json = JsonUtility.ToJson(rankings[i]);
            PlayerPrefs.SetString($"Rank_{i}", json);
        }

        PlayerPrefs.Save();
    }
}
