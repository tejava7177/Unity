using UnityEngine;
using TMPro;
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
    private float gameTime = 0f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (isGameover)
        {
            if (Input.GetMouseButtonDown(0))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        // 게임 시간 누적
        gameTime += Time.deltaTime;

        // 시간 텍스트 UI 업데이트
        timerText.text = $"Time : {gameTime:F2}s";

        // 배경 전환 로직 실행
        UpdateBackground();
    }

    void UpdateBackground()
    {
        if (backgrounds.Length == 0) return;

        // 0~20초 -> index 0, 20~40초 -> index 1, 40~60초 -> index 2, 그 다음 0부터 반복
        int index = (int)(gameTime / 20f) % backgrounds.Length;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].SetActive(i == index);
        }
    }

    public void AddScore(int amount)
    {
        if (isGameover) return;

        score += amount;
        scoreText.text = $"Score : {score}";
    }

    public void OnPlayerDead()
    {
        isGameover = true;
        gameoverUI.SetActive(false);
    }

    public void ReduceTime(float amount)
    {
        gameTime -= amount;
        if (gameTime < 0f) gameTime = 0f;
    }
}