using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class InputFieldDebugger : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("🖱️ InputField 클릭됨!");
    }
}