using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;

    public float spawnIntervalMin = 0.7f;
    public float spawnIntervalMax = 1.2f;

    private float currentInterval;
    private float timer = 0f;

    void Start()
    {
        currentInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentInterval)
        {
            timer = 0f;
            currentInterval = Random.Range(spawnIntervalMin, spawnIntervalMax); // 다음 타이밍 설정

            float camHeight = Camera.main.orthographicSize;
            float camWidth = camHeight * Camera.main.aspect;

            float xPos = Random.Range(-camWidth + 0.5f, camWidth - 0.5f);
            float yPos = camHeight + 1f;

            Vector3 spawnPos = new Vector3(xPos, yPos, 0f);
            Instantiate(balloonPrefab, spawnPos, Quaternion.identity);
        }
    }
}