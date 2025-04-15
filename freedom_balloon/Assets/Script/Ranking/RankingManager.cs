using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

// 플레이어 랭킹을 저장하고 관리하는 클래스
public class RankingManager : MonoBehaviour
{
    public static RankingManager instance;

    private string savePath; // 랭킹 저장 경로

    public List<PlayerScore> rankings = new List<PlayerScore>(); // 랭킹 목록
    private int maxRankCount = 5; // 저장할 최대 랭킹 수

    void Awake()
    {
        // 싱글톤 패턴으로 인스턴스 설정
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 저장 파일 경로 설정
        savePath = Application.persistentDataPath + "/rankings.json";

        // 이전에 저장된 랭킹 불러오기
        LoadRanking();
    }

    // 새로운 점수를 랭킹에 추가하고 저장
    public void AddNewScore(string name, int score)
    {
        PlayerScore newEntry = new PlayerScore(name, score);
        rankings.Add(newEntry);

        // 점수 기준 내림차순 정렬 후 상위 N개만 저장
        rankings = rankings.OrderByDescending(r => r.score).Take(maxRankCount).ToList();

        SaveRanking(); // JSON 저장
    }

    // 랭킹을 JSON 파일로 저장
    void SaveRanking()
    {
        string json = JsonUtility.ToJson(new RankingListWrapper(rankings), true);
        File.WriteAllText(savePath, json);
    }

    // JSON 파일에서 랭킹 불러오기
    void LoadRanking()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            rankings = JsonUtility.FromJson<RankingListWrapper>(json).list;
        }
    }

    // JSON 직렬화를 위한 래퍼 클래스
    [System.Serializable]
    private class RankingListWrapper
    {
        public List<PlayerScore> list;
        public RankingListWrapper(List<PlayerScore> rankings) => list = rankings;
    }

    // 현재 랭킹 리스트 반환
    public List<PlayerScore> GetRankingList()
    {
        return rankings;
    }
}