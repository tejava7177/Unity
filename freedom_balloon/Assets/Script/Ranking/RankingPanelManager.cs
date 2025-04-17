using UnityEngine;
using UnityEngine.EventSystems;

// 랭킹 패널을 제어하는 스크립트
public class RankingPanelManager : MonoBehaviour, IPointerClickHandler
{
    public GameObject rankingPanel; // 랭킹 패널 오브젝트
    public GameObject titleText;    // 타이틀 텍스트 오브젝트

    // 패널 영역 클릭 시 닫기
    public void OnPointerClick(PointerEventData eventData)
    {
        HidePanel();
    }

    // 랭킹 패널 열기
    public void ShowPanel()
    {
        rankingPanel.SetActive(true);
        titleText.SetActive(false); // 타이틀 숨김
        
    }

    // 랭킹 패널 닫기
    public void HidePanel()
    {
        rankingPanel.SetActive(false);
        titleText.SetActive(true); // 타이틀 다시 보이기
    }
}