using UnityEngine;
using UnityEngine.EventSystems;

public class RankingPanelManager : MonoBehaviour, IPointerClickHandler
{
    public GameObject rankingPanel;

    public GameObject titleText;

    public void OnPointerClick(PointerEventData eventData)
    {
        HidePanel();
    }

    public void ShowPanel()
    {
        rankingPanel.SetActive(true);
        titleText.SetActive(false); // ✅ 제목 가리기
    }

    public void HidePanel()
    {
        rankingPanel.SetActive(false);
        titleText.SetActive(true); // ✅ 제목 가리기
    }
}
