using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingUIManager : MonoBehaviour
{
    public GameObject rankingPanel;
    public TextMeshProUGUI[] rankTexts;

    void Start()
    {
        rankingPanel.SetActive(false); // 처음엔 숨김
    }

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
                rankTexts[i].text = $"{i + 1}. ---";
            }
        }

        rankingPanel.SetActive(true);
    }

    public void HideRanking()
    {
        rankingPanel.SetActive(false);
    }
}