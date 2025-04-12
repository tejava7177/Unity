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

            if (col != null && col.CompareTag("Balloon"))
            {
                Balloon balloon = col.GetComponent<Balloon>();

                // 🎯 보너스 풍선인지 먼저 확인!
                if (balloon.balloonType == BalloonType.TimeBonus)
                {
                    Destroy(col.gameObject);
                    GameManager.instance.AddTime(5f); // +5초
                    Debug.Log("⏱️ 시간 보너스 풍선 터치! +5초");
                    return;
                }

                // ✅ 풍선에 Balloon.cs 없거나 떠오르는 중이면 무시
                if (balloon == null || balloon.isLaunching) return;

                Vector3 pos = col.transform.position;


                

                switch (balloon.balloonColor)
                {
                    case Balloon.BalloonColor.Yellow:
                        Destroy(col.gameObject);
                        GameObject newBlue = Instantiate(blueBalloonPrefab, pos, Quaternion.identity);

                        // ✅ 확인 로그 (디버그용)
                        Debug.Log("💙 파란 풍선 생성됨");

                        GameManager.instance.AddScore(10);
                        break;

                    case Balloon.BalloonColor.Blue:
                        Destroy(col.gameObject);
                        GameObject newPurple = Instantiate(purpleBalloonPrefab, pos, Quaternion.identity);

                        Debug.Log("💜 보라 풍선 생성됨");

                        GameManager.instance.AddScore(20);
                        break;

                    case Balloon.BalloonColor.Purple:
                        Destroy(col.gameObject); // 풍선 제거
                        GameManager.instance.AddScore(30); // 점수 +30
                        Debug.Log("🟣 보라 풍선 터치 → +30점!");
                        break;
                }
            }
        }
    }
}