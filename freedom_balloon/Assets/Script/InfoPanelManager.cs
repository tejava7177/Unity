using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class InfoPanelManager : MonoBehaviour, IPointerClickHandler
{
    public GameObject infoPanel;
    public GameObject startCanvas;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("📌 InfoPanel 클릭됨! → 닫기 동작 시작");
        infoPanel.SetActive(false);
        startCanvas.SetActive(true);
    }
}