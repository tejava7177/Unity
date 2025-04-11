using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Balloon : MonoBehaviour
{
    [Header("흔들림 설정")]
    public float swayAmplitude = 0.5f;   // 좌우 흔들림 범위
    public float swayFrequency = 2f;     // 흔들림 속도

    [Header("파란 풍선 설정")]
    public bool isBlue = false;           // 파란 풍선 여부
    public float launchForce = 6f;        // 위로 튕겨 오를 힘

    private Vector3 basePosition;         // 흔들림 기준 위치
    private float swayTimer = 0f;

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
            // 중력은 항상 작용하지만 약하게 설정 (자연스럽게 낙하)
            rb.gravityScale = 0.3f;

            // 위로 한번 AddForce로 튕겨 오름
            rb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);

            Debug.Log($"💙 파란 풍선: AddForce 위로 {launchForce} 적용됨");
        }
    }

    void Update()
    {
        swayTimer += Time.deltaTime;

        // 🌬️ 바람에 좌우 흔들림 (X 기준은 basePosition 기준으로 계산)
        float sway = Mathf.Sin(swayTimer * swayFrequency) * swayAmplitude;
        Vector3 pos = transform.position;
        pos.x = basePosition.x + sway;

        transform.position = pos;
    }
}