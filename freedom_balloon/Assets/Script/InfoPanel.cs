using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public GameObject infoPanel;

    // 호출되면 Panel을 켜고 끄도록 설정
    public void ToggleInfoPanel()
    {
        if (infoPanel != null)
        {
            bool isActive = infoPanel.activeSelf;
            infoPanel.SetActive(!isActive);
        }
    }
}
