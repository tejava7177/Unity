using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Balloon : MonoBehaviour
{

    public enum BalloonColor { Yellow, Blue, Purple }
    public BalloonColor balloonColor = BalloonColor.Yellow;


    [Header("흔들림 설정")]
    public float swayAmplitude = 0.5f;   // 좌우 흔들림 범위
    public float swayFrequency = 2f;     // 흔들림 속도

    [Header("파란 풍선 설정")]
    public bool isBlue = false;           // 파란 풍선 여부
    public float launchForce = 10;        // 위로 튕겨 오를 힘

    private Vector3 basePosition;         // 흔들림 기준 위치
    private float swayTimer = 0f;

    public bool isLaunching = false;

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

        if (balloonColor != BalloonColor.Yellow)
        {
            //rb.gravityScale = 0.3f;
            Debug.Log($"🎈 [{balloonColor}] 풍선 AddForce({launchForce})");
            rb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
            isLaunching = true;
            Debug.Log($"🎈 {balloonColor} 풍선: AddForce({launchForce}) 적용됨");
        }
    }

    void Update()
    {

        if (isLaunching && rb.velocity.y <= 0.1f)
        {
            isLaunching = false;
        }


        swayTimer += Time.deltaTime;

        // 🌬️ 바람에 좌우 흔들림 (X 기준은 basePosition 기준으로 계산)
        float sway = Mathf.Sin(swayTimer * swayFrequency) * swayAmplitude;
        Vector3 pos = transform.position;
        pos.x = basePosition.x + sway;

        transform.position = pos;
    }
}