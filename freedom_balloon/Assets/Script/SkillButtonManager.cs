using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonManager : MonoBehaviour
{
    public float cooldownTime = 30f;
    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;

    [Header("UI")]
    public Image buttonImage;
    public Image fillBackground; // 🩶 회색 배경
    public Image fillForeground; // 🔴 차오르는 색

    public Color activeColor = Color.white;
    public Color cooldownColor = Color.gray;

    void Start()
    {
        SetButtonActive(true);
    }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer += Time.unscaledDeltaTime;
            float ratio = Mathf.Clamp01(cooldownTimer / cooldownTime);
            if (fillForeground != null)
                fillForeground.fillAmount = ratio;

            if (cooldownTimer >= cooldownTime)
            {
                SetButtonActive(true);
            }
        }
    }

    public void OnSkillButtonClick()
    {
        if (isOnCooldown) return;

        Debug.Log("🐶 필살기 버튼 클릭됨!");
        StartCoroutine(ActivateSkill());
    }

    IEnumerator ActivateSkill()
    {
        SetButtonActive(false);
        yield return GameManager.instance.StartCoroutine(GameManager.instance.ExplodeAllBalloons());
    }

    void SetButtonActive(bool isActive)
    {
        isOnCooldown = !isActive;

        if (buttonImage != null)
            buttonImage.color = isActive ? activeColor : cooldownColor;

        if (fillForeground != null)
            fillForeground.fillAmount = isActive ? 1f : 0f;

        if (!isActive)
        {
            cooldownTimer = 0f;
        }
    }
}