using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 랭킹 데이터를 UI에 표시하는 스크립트
public class RankingUIManager : MonoBehaviour
{
    public GameObject rankingPanel;           // 랭킹 패널 오브젝트
    public TextMeshProUGUI[] rankTexts;       // 각 순위에 표시될 텍스트 배열
    public GameObject titleText;              // 타이틀 텍스트 오브젝트

    void Start()
    {
        rankingPanel.SetActive(false);        // 시작 시 랭킹 패널 숨김
    }

    // 랭킹 데이터를 읽어와 UI에 표시
    public void ShowRanking()
    {
        List<PlayerScore> ranks = RankingManager.instance.GetRankingList();

        for (int i = 0; i < rankTexts.Length; i++)
        {
            if (i < ranks.Count)
            {
                rankTexts[i].text = $"{i + 1}. {ranks[i].nickname} - {ranks[i].score}";
            }
            else
            {
                rankTexts[i].text = $"{i + 1}. ---"; // 비어있는 랭킹은 표시 없음
            }
        }

        rankingPanel.SetActive(true);         // 패널 표시
        titleText.SetActive(false);           // 타이틀 숨김
    }

    // 랭킹 패널 닫기
    public void HideRanking()
    {
        rankingPanel.SetActive(false);
        titleText.SetActive(true);            // 타이틀 다시 표시
    }
}