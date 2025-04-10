using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;       // 노란 풍선 프리팹
    public float spawnInterval = 1.5f;     // 생성 주기
    public float xMin = -2.5f, xMax = 2.5f; // X 생성 범위

    private float timer = 0f;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            Vector3 spawnPos = cam.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), 1.1f, 10f));
            Instantiate(balloonPrefab, spawnPos, Quaternion.identity);
        }
    }
}
