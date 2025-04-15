using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

// InfoPanel 클릭 시 닫고, TitleText 토글을 담당하는 매니저
public class InfoPanelManager : MonoBehaviour, IPointerClickHandler
{
    public GameObject infoPanel;      // InfoPanel 객체
    public GameObject titleText;      // 화면 상단 제목 텍스트

    // InfoPanel이 클릭되었을 때 → 패널 닫고 제목 다시 표시
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("📌 InfoPanel 클릭됨! → 닫기 동작 시작");

        infoPanel.SetActive(false);

        if (titleText != null)
            titleText.SetActive(true);
    }

    // 외부에서 InfoPanel을 켤 때 호출되는 함수
    public void ShowInfoPanel()
    {
        infoPanel.SetActive(true);

        if (titleText != null)
            titleText.SetActive(false);

        Debug.Log("📌 InfoPanel 열기 - TitleText 비활성화");
    }
}