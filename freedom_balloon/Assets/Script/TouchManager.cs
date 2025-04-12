using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchManager : MonoBehaviour
{
    public GameObject blueBalloonPrefab;
    public GameObject purpleBalloonPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(worldPos);

            // 💡 터치 위치와 감지된 오브젝트 로그
            Debug.Log("📌 터치 위치 (월드): " + worldPos);

            if (col == null)
            {
                Debug.LogWarning("❌ 터치한 위치에 어떤 콜라이더도 없음!");
                return;
            }

            Debug.Log("✅ 감지된 오브젝트: " + col.name);

            if (!col.CompareTag("Balloon"))
            {
                Debug.LogWarning("🚫 감지된 오브젝트는 Balloon 태그가 아님!");
                return;
            }

            Balloon balloon = col.GetComponent<Balloon>();

            if (balloon == null)
            {
                Debug.LogWarning("⚠️ 감지된 오브젝트에 Balloon.cs가 없음!");
                return;
            }

            Debug.Log("🎈 Balloon Type: " + balloon.balloonType);
            Debug.Log("🎨 Balloon Color: " + balloon.balloonColor);
            Debug.Log("🚀 isLaunching 상태: " + balloon.isLaunching);

            // 🎯 보너스 풍선은 가장 먼저 처리
            if (balloon.balloonType == BalloonType.TimeBonus)
            {
                Destroy(col.gameObject);
                GameManager.instance.AddTime(5f);
                Debug.Log("⏱️ 시간 보너스 풍선 터치! +5초");
                return;
            }

            // 🚫 떠오르는 풍선은 무시
            if (balloon.isLaunching)
            {
                Debug.Log("🛑 떠오르는 중이므로 터치 무시됨.");
                return;
            }

            Vector3 pos = col.transform.position;

            // 🎨 진화 처리
            switch (balloon.balloonColor)
            {
                case Balloon.BalloonColor.Yellow:
                    Destroy(col.gameObject);
                    Instantiate(blueBalloonPrefab, pos, Quaternion.identity);
                    GameManager.instance.AddScore(10);
                    Debug.Log("💙 노랑 → 파랑 풍선 진화! +10점");
                    break;

                case Balloon.BalloonColor.Blue:
                    Destroy(col.gameObject);
                    Instantiate(purpleBalloonPrefab, pos, Quaternion.identity);
                    GameManager.instance.AddScore(20);
                    Debug.Log("💜 파랑 → 보라 풍선 진화! +20점");
                    break;

                case Balloon.BalloonColor.Purple:
                    Destroy(col.gameObject);
                    GameManager.instance.AddScore(30);
                    Debug.Log("🎉 보라 풍선 터치! +30점");
                    break;
            }
        }
    }
}