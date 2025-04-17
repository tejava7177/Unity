using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // 게임 종료 여부
    [Header("게임 상태")]
    public bool isGameover = false;

    // 이름 입력 모드 여부
    public static bool isInNameInputMode = false;

    // 점수 / 타이머 / 게임오버 UI
    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject gameoverUI;

    // 배경 이미지 (낮 / 저녁 / 밤)
    [Header("배경 오브젝트")]
    public GameObject[] backgrounds;

    // 이름 입력창 관리 스크립트
    public NameInputManager nameInputManager;

    // 풍선 속도 관련 설정
    [Header("풍선 속도 제어")]
    public float normalGravityScale = 0.5f;
    public float slowGravityScale = 0.1f;
    public float slowDuration = 5f;
    private Coroutine slowEffectCoroutine;

    // 점수 2배 효과 관련
    private bool isDoubleScore = false;
    private float doubleScoreDuration = 7f;
    private float doubleScoreTimer = 0f;

    // 점수 및 타이머
    private int score = 0;
    [SerializeField] private float maxTime = 60f;
    private float currentTime;

    // 타이머 깜빡임 여부
    private bool isBlinking = false;
    public GameObject dogSkillButton; // Inspector 연결 필요


    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentTime = maxTime;
        UpdateScoreText();
    }

    void Update()
    {
        // 게임 종료 시 → 클릭으로 씬 리셋
        if (isGameover)
        {
            if (Input.GetMouseButtonDown(0))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        // 타이머 감소 및 게임 종료 체크
        currentTime -= Time.deltaTime;
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            OnPlayerDead();
        }

        // 10초 이하 시 타이머 깜빡임
        if (currentTime <= 10f && !isBlinking)
        {
            StartCoroutine(BlinkTimerText());
        }

        // 점수 2배 상태 타이머 감소
        if (isDoubleScore)
        {
            doubleScoreTimer -= Time.deltaTime;
            if (doubleScoreTimer <= 0f)
                isDoubleScore = false;
        }

        // 타이머 텍스트 업데이트
        int seconds = (int)currentTime % 60;
        int milliseconds = (int)((currentTime % 1f) * 100);
        timerText.text = $"Time : {seconds:00}.{milliseconds:00}s";

        // 배경 이미지 업데이트
        UpdateBackground();
    }

    // 배경 전환 + 텍스트 색상 조정
    void UpdateBackground()
    {
        if (backgrounds.Length == 0) return;

        float elapsedTime = maxTime - currentTime;
        int index = (int)(elapsedTime / 20f) % backgrounds.Length;

        for (int i = 0; i < backgrounds.Length; i++)
            backgrounds[i].SetActive(i == index);

        // 밤일 경우 텍스트를 흰색으로
        bool isNight = (index == backgrounds.Length - 1);
        scoreText.color = isNight ? Color.white : Color.black;
        timerText.color = isNight ? Color.white : Color.black;
    }

    // 게임 종료 처리
    public void OnPlayerDead()
    {
        isGameover = true;
        gameoverUI.SetActive(true);
        if (dogSkillButton != null)
            dogSkillButton.SetActive(false);  // ← 여기 추가
        nameInputManager.Show();
    }

    // 시간 감소 함수
    public void ReduceTime(float amount)
    {
        currentTime -= amount;
        if (currentTime < 0f)
            currentTime = 0f;
    }

    // 타이머 깜빡임 효과
    private IEnumerator BlinkTimerText()
    {
        isBlinking = true;
        Color normalColor = Color.white;
        Color warningColor = Color.red;

        while (currentTime > 0)
        {
            timerText.color = warningColor;
            yield return new WaitForSeconds(0.3f);
            timerText.color = normalColor;
            yield return new WaitForSeconds(0.3f);
        }

        timerText.color = warningColor;
    }

    // 점수 추가 처리
    public void AddScore(int amount)
    {
        if (isDoubleScore) amount *= 2;
        score += amount;
        scoreText.text = $"Score : {score}";
    }

    // 시간 추가 처리
    public void AddTime(float time)
    {
        currentTime += time;
        if (currentTime > maxTime)
            currentTime = maxTime;
    }

    // 점수 2배 효과 시작
    public void ActivateDoubleScore()
    {
        isDoubleScore = true;
        doubleScoreTimer = doubleScoreDuration;
    }

    // 점수 텍스트 갱신
    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = $"Score : {score}";
    }

    // 현재 점수 반환
    public int GetScore() => score;

    // 풍선 낙하 속도 느려짐 적용
    public void ApplySlowEffect()
    {
        if (slowEffectCoroutine != null)
            StopCoroutine(slowEffectCoroutine);
        slowEffectCoroutine = StartCoroutine(SlowEffectRoutine());
    }

    // 풍선 느려짐 효과 루틴
    private IEnumerator SlowEffectRoutine()
    {
        Balloon[] allBalloons = FindObjectsOfType<Balloon>();
        foreach (Balloon balloon in allBalloons)
        {
            if (balloon.balloonType == BalloonType.Normal &&
                balloon.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                rb.gravityScale = slowGravityScale;
        }

        yield return new WaitForSeconds(slowDuration);

        foreach (Balloon balloon in FindObjectsOfType<Balloon>())
        {
            if (balloon.balloonType == BalloonType.Normal &&
                balloon.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                rb.gravityScale = normalGravityScale;
        }

        slowEffectCoroutine = null;
    }

    // 모든 풍선 터뜨리기 루틴
    public IEnumerator ExplodeAllBalloons()
    {
        Time.timeScale = 0f;

        List<Balloon> balloons = new List<Balloon>(FindObjectsOfType<Balloon>());
        balloons.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        foreach (Balloon b in balloons)
        {
            TouchManager.SimulateBalloonTouch(b);
            yield return new WaitForSecondsRealtime(0.1f);
        }

        Time.timeScale = 1f;
    }

    // 게임이 플레이 가능한 상태인지 여부
    public bool IsGamePlayable()
    {
        return this.enabled && !isGameover && !GameManager.isInNameInputMode;
    }
}