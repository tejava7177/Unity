using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 일정 간격으로 다양한 종류의 풍선을 생성하는 매니저
public class BalloonSpawner : MonoBehaviour
{
    [Header("풍선 프리팹")]
    public GameObject normalBalloonPrefab;
    public GameObject timeBonusBalloonPrefab;
    public GameObject scoreBonusBalloonPrefab;
    public GameObject slowBalloonPrefab;

    [Header("생성 간격")]
    public float spawnIntervalMin = 0.7f;
    public float spawnIntervalMax = 1.2f;

    [Header("보너스 풍선 확률 (0~1)")]
    [Range(0f, 1f)] public float timeBonusChance = 0.1f;
    [Range(0f, 1f)] public float scoreBonusChance = 0.05f;
    [Range(0f, 1f)] public float slowBalloonChance = 0.05f;

    private float currentInterval;
    private float timer = 0f;

    void Start()
    {
        SetNextInterval(); // 첫 생성 간격 설정
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentInterval)
        {
            timer = 0f;
            SetNextInterval(); // 다음 생성 간격 설정
            SpawnBalloon();    // 풍선 생성
        }
    }

    // 다음 생성까지의 간격을 랜덤 설정
    void SetNextInterval()
    {
        currentInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    // 화면 상단 무작위 위치에 풍선 생성
    void SpawnBalloon()
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        float xPos = Random.Range(-camWidth + 0.5f, camWidth - 0.5f);
        float yPos = camHeight + 1f;

        Vector3 spawnPos = new Vector3(xPos, yPos, 0f);

        GameObject prefabToSpawn;

        // 보너스 풍선 확률 기반 선택
        float rand = Random.value;

        if (rand <= slowBalloonChance)
            prefabToSpawn = slowBalloonPrefab;
        else if (rand <= slowBalloonChance + timeBonusChance)
            prefabToSpawn = timeBonusBalloonPrefab;
        else if (rand <= slowBalloonChance + timeBonusChance + scoreBonusChance)
            prefabToSpawn = scoreBonusBalloonPrefab;
        else
            prefabToSpawn = normalBalloonPrefab;

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
}