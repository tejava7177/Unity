using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class InfoPanelManager : MonoBehaviour, IPointerClickHandler
{
    public GameObject infoPanel;
    //public GameObject startCanvas;
    public GameObject titleText; // 🎯 TitleText 연결

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("📌 InfoPanel 클릭됨! → 닫기 동작 시작");

        infoPanel.SetActive(false);
        //startCanvas.SetActive(true);

        if (titleText != null)
            titleText.SetActive(true); // ✅ 다시 보이게!
    }

    // 🎯 InfoPanel을 열 때 호출할 함수 (필요한 경우)
    public void ShowInfoPanel()
    {
        infoPanel.SetActive(true);
        //startCanvas.SetActive(false);

        if (titleText != null)
            titleText.SetActive(false); // ✅ 제목 가리기

        Debug.Log("📌 InfoPanel 열기 - TitleText 비활성화");

    }



}