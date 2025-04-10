using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Balloon : MonoBehaviour
{
    public float swayAmplitude = 0.5f;   // 좌우 흔들림 범위
    public float swayFrequency = 2f;     // 흔들림 속도

    public bool isBlue = false;          // 파란 풍선 여부
    public float riseHeight = 1.2f;        // 얼마나 위로 솟구칠지
    public float riseSpeed = 0.05f;       // 🎯 현실적인 속도 설정

    private Vector3 startPos;
    private float riseTargetY;
    private bool rising = false;
    private float swayTimer = 0f;
    private Vector3 basePosition; // 기준 위치 (초기 위치)
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("❌ Rigidbody2D 없음! 오브젝트 이름: " + gameObject.name);
            return;
        }

        basePosition = transform.position;

        if (isBlue)
        {
            rb.gravityScale = 0f;
            rising = true;
            riseTargetY = basePosition.y + riseHeight;
        }
    }



    void Update()
    {
        swayTimer += Time.deltaTime;

        Vector3 pos = transform.position;

        if (rising)
        {
            pos.y += riseSpeed * Time.deltaTime;
            if (pos.y >= riseTargetY)
            {
                pos.y = riseTargetY;
                rb.gravityScale = 1f;
                rising = false;
            }
        }

        // 🌬️ 흔들림: 기준 X 좌표 + 진동
        float sway = Mathf.Sin(swayTimer * swayFrequency) * swayAmplitude;
        pos.x = basePosition.x + sway;

        transform.position = pos;
    }



    IEnumerator InitializeStartPos()
    {
        yield return null; // 1프레임 대기

        startPos = transform.position;

        if (isBlue)
        {
            rb.gravityScale = 0f;
            rising = true;
            riseTargetY = startPos.y + riseHeight;
            Debug.Log($"💙 파란 풍선: 떠오르기 시작! 목표 높이 = {riseTargetY}");
        }
    }


}