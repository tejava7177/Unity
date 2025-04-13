using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonManager : MonoBehaviour
{
    public float cooldownTime = 30f;
    private bool isOnCooldown = false;

    public Image buttonImage; // 🟡 UI Button Image 연결
    public Color activeColor = Color.white;
    public Color cooldownColor = Color.gray;

    void Start()
    {
        SetButtonActive(true);
    }

    public void OnSkillButtonClick() // 🎯 버튼 클릭 연결용 함수
    {
        if (isOnCooldown) return;

        Debug.Log("🐶 필살기 버튼 클릭됨!");
        StartCoroutine(ActivateSkill());
    }

    IEnumerator ActivateSkill()
    {
        SetButtonActive(false);
        yield return GameManager.instance.StartCoroutine(GameManager.instance.ExplodeAllBalloons());
        yield return new WaitForSecondsRealtime(cooldownTime);
        SetButtonActive(true);
    }

    void SetButtonActive(bool isActive)
    {
        isOnCooldown = !isActive;
        if (buttonImage != null)
            buttonImage.color = isActive ? activeColor : cooldownColor;
    }
}