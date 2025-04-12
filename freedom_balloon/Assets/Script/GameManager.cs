using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("게임 상태")]
    public bool isGameover = false;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject gameoverUI;

    [Header("배경 오브젝트")]
    public GameObject[] backgrounds; // 낮-저녁-밤 순으로 넣기


    [Header("풍선 속도 제어")]
    public float normalGravityScale = 0.5f;
    public float slowGravityScale = 0.2f;
    public float slowDuration = 5f;

    private Coroutine slowEffectCoroutine;



    // 🔥 점수 2배 상태 관련 변수
    private bool isDoubleScore = false;
    private float doubleScoreDuration = 7f;
    private float doubleScoreTimer = 0f;



    private int score = 0;
    [SerializeField] private float maxTime = 60f;
    private float currentTime;


    private bool isBlinking = false; //깜빡임 추가

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
        if (isGameover)
        {
            if (Input.GetMouseButtonDown(0))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        // 타이머 감소
        currentTime -= Time.deltaTime;
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            OnPlayerDead();
        }


        if (currentTime <= 10f && !isBlinking)
        {
            StartCoroutine(BlinkTimerText());
        }


        // ⏳ 점수 2배 타이머 처리
        if (isDoubleScore)
        {
            doubleScoreTimer -= Time.deltaTime;
            if (doubleScoreTimer <= 0f)
            {
                isDoubleScore = false;
                Debug.Log("🔚 점수 2배 효과 종료");
            }
        }

        int seconds = (int)currentTime % 60;
        int milliseconds = (int)((currentTime % 1f) * 100);

        timerText.text = $"Time : {seconds:00}.{milliseconds:00}s";

        // 배경 전환
        UpdateBackground();
    }

    void UpdateBackground()
    {
        if (backgrounds.Length == 0) return;

        // 경과 시간 기준으로 배경 전환
        float elapsedTime = maxTime - currentTime;
        int index = (int)(elapsedTime / 20f) % backgrounds.Length;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].SetActive(i == index);
        }
    }

   

    public void OnPlayerDead()
    {
        isGameover = true;
        gameoverUI.SetActive(true);
    }

    public void ReduceTime(float amount)
    {
        currentTime -= amount;
        if (currentTime < 0f)
            currentTime = 0f;
    }


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

        // 시간 다 지나면 마지막 색상 고정
        timerText.color = warningColor;
    }



    public void AddScore(int amount)
    {
        if (isDoubleScore)
        {
            amount *= 2;
            Debug.Log($"💥 점수 2배 적용! +{amount}");
        }

        score += amount;
        scoreText.text = $"Score : {score}";
    }

    public void AddTime(float time)
    {
        currentTime += time;
        if (currentTime > maxTime)
            currentTime = maxTime; // 최대 제한 (선택)

        Debug.Log($"⏱️ 시간 추가됨! 현재 시간: {currentTime:F1}s");
    }

    // 점수 2배 발동 함수
    public void ActivateDoubleScore()
    {
        isDoubleScore = true;
        doubleScoreTimer = doubleScoreDuration;
        Debug.Log("🔥 점수 2배 효과 시작 (7초)");
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score : {score}";
        }
    }

    public int GetScore() => score;



    public void ApplySlowEffect()
    {
        if (slowEffectCoroutine != null)
            StopCoroutine(slowEffectCoroutine);

        slowEffectCoroutine = StartCoroutine(SlowEffectRoutine());
    }

    private IEnumerator SlowEffectRoutine()
    {
        Debug.Log("🐌 풍선 속도 느려짐 시작");

        // 현재 씬의 모든 풍선 확인
        Balloon[] allBalloons = FindObjectsOfType<Balloon>();
        foreach (Balloon balloon in allBalloons)
        {
            // ✅ Slow 풍선은 제외
            if (balloon.balloonType == BalloonType.Normal)
            {
                if (balloon.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    rb.gravityScale = slowGravityScale; // 예: 0.1f
                    Debug.Log($"⬇️ 느려진 풍선: {balloon.name}");
                }
            }
        }

        yield return new WaitForSeconds(slowDuration); // 예: 5초

        // 다시 원래 속도로 복귀
        allBalloons = FindObjectsOfType<Balloon>();
        foreach (Balloon balloon in allBalloons)
        {
            if (balloon.balloonType == BalloonType.Normal)
            {
                if (balloon.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    rb.gravityScale = normalGravityScale; // 예: 0.5f
                    Debug.Log($"↗️ 원래 속도 복귀: {balloon.name}");
                }
            }
        }

        Debug.Log("🐌 풍선 속도 느려짐 종료");
        slowEffectCoroutine = null;
    }




}