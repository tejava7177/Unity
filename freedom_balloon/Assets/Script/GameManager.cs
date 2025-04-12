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
        score += amount;
        UpdateScoreText();
    }

    public void AddTime(float time)
    {
        currentTime += time;
        if (currentTime > maxTime)
            currentTime = maxTime; // 최대 제한 (선택)

        Debug.Log($"⏱️ 시간 추가됨! 현재 시간: {currentTime:F1}s");
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score : {score}";
        }
    }

    public int GetScore() => score;
}