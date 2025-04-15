using UnityEngine;
using UnityEngine.EventSystems;

public class RankingPanelManager : MonoBehaviour, IPointerClickHandler
{
    public GameObject rankingPanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        HidePanel();
    }

    public void ShowPanel()
    {
        rankingPanel.SetActive(true);
    }

    public void HidePanel()
    {
        rankingPanel.SetActive(false);
    }
}
