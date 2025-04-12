using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    [Header("풍선 프리팹")]
    public GameObject normalBalloonPrefab;
    public GameObject timeBonusBalloonPrefab;

    [Header("생성 간격")]
    public float spawnIntervalMin = 0.7f;
    public float spawnIntervalMax = 1.2f;

    [Header("보너스 풍선 확률 (0~1)")]
    [Range(0f, 1f)] public float timeBonusChance = 0.1f; // 10% 확률

    private float currentInterval;
    private float timer = 0f;

    void Start()
    {
        SetNextInterval();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentInterval)
        {
            timer = 0f;
            SetNextInterval();

            SpawnBalloon();
        }
    }

    void SetNextInterval()
    {
        currentInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    void SpawnBalloon()
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        float xPos = Random.Range(-camWidth + 0.5f, camWidth - 0.5f);
        float yPos = camHeight + 1f;

        Vector3 spawnPos = new Vector3(xPos, yPos, 0f);

        GameObject prefabToSpawn;

        // 🎯 확률 기반 보너스 풍선 생성
        float rand = Random.value;
        if (rand <= timeBonusChance)
        {
            prefabToSpawn = timeBonusBalloonPrefab;
        }
        else
        {
            prefabToSpawn = normalBalloonPrefab;
        }

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
}