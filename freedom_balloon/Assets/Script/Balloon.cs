using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 풍선의 타입 정의 (기본, 보너스, 특수효과 등)
public enum BalloonType
{
    Normal,     // 일반 풍선
    TimeBonus,  // 시간 보너스 풍선
    ScoreBonus, // 점수 2배 풍선
    SlowEffect  // 낙하 속도 감소 풍선
}

// 풍선 동작 제어 스크립트
public class Balloon : MonoBehaviour
{
    // 풍선 색상 (진화 단계)
    public enum BalloonColor { Yellow, Blue, Purple }

    public BalloonType balloonType = BalloonType.Normal;
    public BalloonColor balloonColor = BalloonColor.Yellow;

    [Header("흔들림 설정")]
    public float swayAmplitude = 0.5f;
    public float swayFrequency = 2f;

    [Header("위로 튀는 힘 설정")]
    public bool isBlue = false; // 사용 안됨 (백업용?)
    public float launchForce = 10f;

    private Vector3 basePosition;     // 흔들림 기준 위치
    private float swayTimer = 0f;

    public bool isLaunching = false; // 위로 튀는 중 여부

    private Rigidbody2D rb;

    [Header("효과음")]
    public AudioClip popSFX;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D가 존재하지 않습니다.");
            return;
        }

        basePosition = transform.position;

        // 파랑/보라 풍선은 위로 발사됨
        if (balloonColor != BalloonColor.Yellow)
        {
            rb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
            isLaunching = true;
            gameObject.layer = LayerMask.NameToLayer("BalloonUp");
        }
        else
        {
            // 노랑 풍선은 아래로 낙하 시작
            gameObject.layer = LayerMask.NameToLayer("BalloonDown");
        }
    }

    void Update()
    {
        // 위로 올라갔다가 멈췄으면 낙하 상태로 전환
        if (isLaunching && rb.velocity.y <= 0.1f)
        {
            isLaunching = false;
            gameObject.layer = LayerMask.NameToLayer("BalloonDown");
        }

        // 좌우 흔들림 애니메이션 적용
        swayTimer += Time.deltaTime;
        float sway = Mathf.Sin(swayTimer * swayFrequency) * swayAmplitude;
        Vector3 pos = transform.position;
        pos.x = basePosition.x + sway;
        transform.position = pos;
    }

    // 풍선 터뜨리기 (효과음 재생 후 제거)
    public void Pop()
    {
        if (popSFX != null && BalloonSFXManager.instance != null)
        {
            BalloonSFXManager.instance.PlayPopSound(transform.position);
        }

        Destroy(gameObject);
    }
}