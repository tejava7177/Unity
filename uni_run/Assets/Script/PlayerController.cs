using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioClip deathClip;              //사망시 재생할 오디오 클립
    public float jumpForce = 700f;           //점프 힘

    private int jumpCount = 0;               //누적 점프 횟수
    private bool isGrounded = false;         //바닥에 닿았는지
    private bool isDead = false;             //사망 상태

    private Rigidbody2D playerRigidbody;     //사용할 리지드바디 컴포넌트
    private Animator animator;               //사용할 애니메이터 컴포넌트
    private AudioSource playerAudio;         //사용할 오디오 소스 컴포넌트


    public int maxHP = 5;                  // 최대 체력
    private int currentHP;                   // 현재 체력
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        //초기화
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 체력 초기화 및 UI 반영
        currentHP = maxHP;
        GameManager.instance.UpdateHPUI(currentHP, maxHP);
    }

    void Update()
    {
        //사용자 입력을 감지하고 점프하는 처리
        if (isDead)
        {
            return;         //사망 시 처리를 더 이상 진행하지 않고 종료
        }
        if (Input.GetMouseButtonDown(0) && jumpCount < 2)
        {
            jumpCount++;           //점프 횟수 증가
            playerRigidbody.velocity = Vector2.zero;                //점프 직전에 속도를 순간적으로 제로로 변경
            playerRigidbody.AddForce(new Vector2(0, jumpForce));    //리지드 바디 위쪽으로 힘주기
            playerAudio.Play();                                     //오디오소스재생
        }
        else if (Input.GetMouseButtonUp(0) && playerRigidbody.velocity.y > 0)
        {
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f;     //현재 속도를 절반으로 변경
        }
        animator.SetBool("Grounded", isGrounded);           //애니메이터의 Grounded 파라미터를 isFrounded  값으로 갱신
    }

    // 데미지를 받는 함수
    public void TakeDamage(int damage)
    {
        if (isDead || currentHP <= 0)
            return;

        currentHP -= damage;
        GameManager.instance.UpdateHPUI(currentHP, maxHP);

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    // 플레이어가 사망했을 때 호출되는 함수
    private void Die()
    {
        // 이미 죽은 상태면 다시 실행하지 않음 (중복 방지)
        if (isDead) return;

        // 사망 상태로 설정하여 이후 로직에서 입력 및 동작 차단
        isDead = true;

        // 애니메이터에 "Die" 트리거를 전달하여 사망 애니메이션 재생
        animator.SetTrigger("Die");

        // 사망 사운드 클립으로 오디오 설정 후 재생
        playerAudio.clip = deathClip;
        playerAudio.Play();

        // 현재 속도를 0으로 초기화하여 이동을 멈춤
        playerRigidbody.velocity = Vector2.zero;

        // 중력을 꺼서 이후 연출(튕김 및 낙하)을 직접 제어할 수 있도록 설정
        playerRigidbody.isKinematic = true;

        // 사망 후 연출(위로 튕긴 뒤 천천히 낙하)을 처리하는 코루틴 실행
        StartCoroutine(DeathAnimationSequence());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Dead" && !isDead)
        {
            Die();
        }
    }

    // 장애물 충돌 시 데미지 적용
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(1); // 장애물 1회 충돌 시 1 데미지
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }


    // 사망 연출을 처리하는 코루틴 (위로 튀었다가 천천히 아래로 떨어짐)
    IEnumerator DeathAnimationSequence()
    {
        
        // 1. 위로 튀어오르는 연출
        // 튕겨 올라갈 높이 설정 (4.5 유닛 위로)
        float bounceHeight = 4.5f;

        // 튀어오르는 데 걸리는 시간 (초 단위)
        float bounceTime = 1.5f;

        // 목표 위치: 현재 위치에서 위로 bounceHeight만큼 이동한 지점
        Vector3 upPosition = transform.position + Vector3.up * bounceHeight;

        // 경과 시간 초기화
        float elapsed = 0f;

        // 시작 위치 저장
        Vector3 start = transform.position;

        // bounceTime 동안 위로 부드럽게 이동
        while (elapsed < bounceTime)
        {
            // Lerp를 사용해 현재 위치를 보간 이동
            transform.position = Vector3.Lerp(start, upPosition, elapsed / bounceTime);

            // 시간 누적
            elapsed += Time.deltaTime;

            // 프레임 대기
            yield return null;
        }

        
        // 2. 아래로 천천히 떨어지는 연출
        // 떨어지는 데 걸리는 시간
        float fallTime = 1.2f;

        // 최종 목표 위치: 튀어오른 위치에서 아래로 5 유닛만큼 내려간 지점
        Vector3 endPosition = upPosition + Vector3.down * 5f;

        // 시간과 시작 위치 다시 초기화
        elapsed = 0f;
        start = transform.position;

        // fallTime 동안 부드럽게 아래로 이동
        while (elapsed < fallTime)
        {
            transform.position = Vector3.Lerp(start, endPosition, elapsed / fallTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 3. 연출 종료 후 게임 오버 처리
     
        // 연출이 끝나면 GameManager에게 사망 처리 알림
        GameManager.instance.OnPlayerDead();



    }


    // 일정 시간 동안 무적 상태를 유지하고, 그동안 장애물과의 충돌을 무시하며 깜빡이는 시각 효과를 제공하는 코루틴
    private IEnumerator InvincibilityCoroutine()
    {
        // 무적 상태 시작
        isInvincible = true;

        // 플레이어 자신의 Collider를 가져옴
        Collider2D playerCollider = GetComponent<Collider2D>();

        // 현재 씬 내의 모든 Collider2D를 찾음
        Collider2D[] obstacleColliders = FindObjectsOfType<Collider2D>();

        // 장애물들과의 충돌을 일시적으로 무시 설정
        foreach (var col in obstacleColliders)
        {
            if (col.CompareTag("Obstacle"))
            {
                // 플레이어와 장애물 간 충돌 비활성화
                Physics2D.IgnoreCollision(playerCollider, col, true);
            }
        }

        // 깜빡이기 효과 시작
        float blinkTime = 0.15f;             // 깜빡이는 주기 (한 번 보였다/안 보였다 하는 시간)
        float totalBlinkDuration = 0.6f;     // 전체 깜빡이기 지속 시간
        float elapsed = 0f;

        // 지정한 시간 동안 깜빡이는 효과를 반복
        while (elapsed < totalBlinkDuration)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f); // 반투명으로 변경 (보호 상태 느낌)
            yield return new WaitForSeconds(blinkTime);         // 대기

            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);   // 원래 색으로 복원
            yield return new WaitForSeconds(blinkTime);         // 대기

            // 전체 경과 시간 업데이트
            elapsed += blinkTime * 2f;
        }

        // 무적 시간 종료 → 충돌 다시 활성화
        foreach (var col in obstacleColliders)
        {
            if (col.CompareTag("Obstacle"))
            {
                // 플레이어와 장애물 간 충돌 다시 활성화
                Physics2D.IgnoreCollision(playerCollider, col, false);
            }
        }

        // 무적 상태 해제
        isInvincible = false;
    }
}
