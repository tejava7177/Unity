using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("게임 상태")]
    public bool isGameover = false;

    [Header("UI")]
    public TextMeshProUGUI scoreText;      // 점수 텍스트
    public TextMeshProUGUI timerText;      // 생존 시간 텍스트
    public GameObject gameoverUI;          // 게임 오버 UI 패널
    public TextMeshProUGUI hpText;

    private int score = 0;
    private float playTime = 0f;           // 생존 시간

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("씬에 두 개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 게임 오버가 아닌 동안 시간과 입력 체크
        if (!isGameover)
        {
            // 생존 시간 누적
            playTime += Time.deltaTime;
            // 텍스트에 소수점 둘째 자리까지 표시
            timerText.text = $"Time : {playTime:F2}s";
        }
        else
        {
            // 게임 오버 상태에서 마우스 클릭 시 씬 리스타트
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    // 점수를 추가하고 텍스트 갱신
    public void AddScore(int newScore)
    {
        if (!isGameover)
        {
            score += newScore;
            scoreText.text = "Score : " + score;
        }
    }

    // 플레이어가 사망했을 때 호출되는 메서드
    public void OnPlayerDead()
    {
        isGameover = true;
        gameoverUI.SetActive(true);
    }

    public void UpdateHPUI(int current, int max)
    {
        if (hpText != null)
        {
            hpText.text = $"HP : {current} / {max}";
        }
    }

}