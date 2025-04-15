using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 스킬 버튼의 쿨다운 처리 및 시각적 피드백을 담당하는 매니저
public class SkillButtonManager : MonoBehaviour
{
    public float cooldownTime = 30f;                // 쿨타임 지속 시간
    private float cooldownTimer = 0f;               // 현재 경과 시간
    private bool isOnCooldown = false;              // 쿨타임 여부

    [Header("UI")]
    public Image buttonImage;                       // 버튼 이미지
    public Image fillBackground;                    // 회색 배경 이미지
    public Image fillForeground;                    // 아래서 차오르는 색상 이미지

    public Color activeColor = Color.white;         // 쿨타임 끝나고 버튼 색
    public Color cooldownColor = Color.gray;        // 쿨타임 중 버튼 색

    public AudioClip boomSFX; // 💣 필살기 사운드
    private AudioSource audioSource;

    void Start()
    {
        SetButtonActive(true); // 시작 시 버튼 활성화 상태로 설정
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 쿨다운 진행 중이면 시간 업데이트 및 Fill 애니메이션 적용
        if (isOnCooldown)
        {
            cooldownTimer += Time.unscaledDeltaTime;
            float ratio = Mathf.Clamp01(cooldownTimer / cooldownTime);

            if (fillForeground != null)
                fillForeground.fillAmount = ratio;

            // 쿨다운 완료 시 버튼 활성화
            if (cooldownTimer >= cooldownTime)
            {
                SetButtonActive(true);
            }
        }
    }

    // 버튼 클릭 시 호출되는 함수
    public void OnSkillButtonClick()
    {
        if (isOnCooldown) return;

        Debug.Log("🐶 필살기 버튼 클릭됨!");
        StartCoroutine(ActivateSkill());
    }

    // 스킬 실행 및 쿨다운 시작
    IEnumerator ActivateSkill()
    {
        // 💥 사운드 먼저 재생
        if (boomSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(boomSFX);
        }
        SetButtonActive(false);
        yield return GameManager.instance.StartCoroutine(GameManager.instance.ExplodeAllBalloons());
    }

    // 버튼의 활성/비활성 상태 및 시각 효과 처리
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