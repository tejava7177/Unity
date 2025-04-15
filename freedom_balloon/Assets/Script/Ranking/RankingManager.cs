using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class RankingManager : MonoBehaviour
{
    public static RankingManager instance;

    private string savePath;

    public List<PlayerScore> rankings = new List<PlayerScore>();
    private int maxRankCount = 5;

    void Awake()
    {
        Debug.Log("🟡 RankingManager.Awake() 진입");

        if (instance == null)
        {
            instance = this;
            Debug.Log("✅ RankingManager instance 등록 완료");
        }
        else
        {
            Debug.LogWarning("⚠️ 이미 RankingManager 인스턴스가 존재하여 삭제됨");
            Destroy(gameObject);
            return;
        }

        savePath = Application.persistentDataPath + "/rankings.json";
        Debug.Log("📁 랭킹 저장 위치: " + savePath);

        LoadRanking();
    }

    public void AddNewScore(string name, int score)
    {
        PlayerScore newEntry = new PlayerScore(name, score);
        rankings.Add(newEntry);

        // 높은 점수 순 정렬 후 상위 5개만 저장
        rankings = rankings.OrderByDescending(r => r.score).Take(maxRankCount).ToList();

        SaveRanking();

        Debug.Log($"💾 랭킹 저장 완료! {name} - {score}");
    }

    void SaveRanking()
    {
        string json = JsonUtility.ToJson(new RankingListWrapper(rankings), true);
        File.WriteAllText(savePath, json);
    }

    void LoadRanking()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            rankings = JsonUtility.FromJson<RankingListWrapper>(json).list;
        }
    }

    // JSON으로 리스트 저장을 위한 래퍼
    [System.Serializable]
    private class RankingListWrapper
    {
        public List<PlayerScore> list;
        public RankingListWrapper(List<PlayerScore> rankings) => list = rankings;
    }

    public List<PlayerScore> GetRankingList()
    {
        return rankings;
    }


    // [ContextMenu("🔥 디버그용 랭킹 출력")]
    // public void DebugPrintRanking()
    // {
    //     foreach (var r in rankings)
    //     {
    //         Debug.Log($"{r.name} - {r.score}");
    //     }
    // }
}