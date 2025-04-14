using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public GameObject blueBalloonPrefab;
    public GameObject purpleBalloonPrefab;

    void Update()
    {
        // ✅ 게임 가능 상태 아닐 경우 (게임 시작 전, 이름 입력 중 등)
        if (!GameManager.instance.IsGamePlayable())
            return;

        // ✅ 마우스/터치 입력 감지
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverBlockedUI())
            {
                Debug.Log("⚠️ UI 클릭 감지됨 (Button, InputField 제외) → 터치 무시");
                return;
            }

            HandleBalloonTouch();
        }

        
    }

    void HandleBalloonTouch()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D col = Physics2D.OverlapPoint(worldPos);

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
            Debug.LogWarning("⚠️ 감지된 오브젝트에 Balloon 스크립트 없음!");
            return;
        }

        if (balloon.isLaunching)
        {
            Debug.Log("🛑 떠오르는 중인 풍선 → 무시");
            return;
        }

        // 보너스 풍선 처리
        switch (balloon.balloonType)
        {
            case BalloonType.TimeBonus:
                GameManager.instance.AddTime(5f);
                Destroy(balloon.gameObject);
                Debug.Log("⏱️ 시간 보너스 풍선! +5초");
                return;

            case BalloonType.ScoreBonus:
                GameManager.instance.ActivateDoubleScore();
                Destroy(balloon.gameObject);
                Debug.Log("💫 점수 2배 풍선!");
                return;

            case BalloonType.SlowEffect:
                GameManager.instance.ApplySlowEffect();
                Destroy(balloon.gameObject);
                Debug.Log("🐌 풍선 속도 느려짐!");
                return;
        }

        // 일반 풍선 진화
        Vector3 pos = balloon.transform.position;

        switch (balloon.balloonColor)
        {
            case Balloon.BalloonColor.Yellow:
                Instantiate(blueBalloonPrefab, pos, Quaternion.identity);
                GameManager.instance.AddScore(10);
                Debug.Log("💙 노랑 → 파랑 +10점");
                break;

            case Balloon.BalloonColor.Blue:
                Instantiate(purpleBalloonPrefab, pos, Quaternion.identity);
                GameManager.instance.AddScore(20);
                Debug.Log("💜 파랑 → 보라 +20점");
                break;

            case Balloon.BalloonColor.Purple:
                GameManager.instance.AddScore(30);
                Debug.Log("🎉 보라 풍선 +30점");
                break;
        }

        Destroy(balloon.gameObject);
    }

    /// <summary>
    /// UI 위에 터치됐는지 체크하고, Button/InputField는 예외 허용
    /// </summary>
    bool IsTouchOnBlockedUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            GameObject target = result.gameObject;

            // ✅ Button, InputField는 통과시킴
            if (target.GetComponent<Button>() != null ||
                target.GetComponent<InputField>() != null ||
                target.GetComponent<TMP_InputField>() != null)
            {
                return false;
            }
        }

        // ❌ UI 위이지만 Button/InputField가 아님 → 터치 차단
        return results.Count > 0;
    }

    // 자동 터치 처리용 (전체 제거, 스킬 등에서 사용)
    public static void SimulateBalloonTouch(Balloon balloon)
    {
        if (balloon == null) return;

        switch (balloon.balloonType)
        {
            case BalloonType.TimeBonus:
                GameManager.instance.AddTime(5f);
                break;

            case BalloonType.ScoreBonus:
                GameManager.instance.ActivateDoubleScore();
                break;

            case BalloonType.SlowEffect:
                GameManager.instance.ApplySlowEffect();
                break;
        }

        switch (balloon.balloonColor)
        {
            case Balloon.BalloonColor.Yellow:
                GameManager.instance.AddScore(10);
                break;

            case Balloon.BalloonColor.Blue:
                GameManager.instance.AddScore(20);
                break;

            case Balloon.BalloonColor.Purple:
                GameManager.instance.AddScore(30);
                break;
        }

        Destroy(balloon.gameObject);
    }


    bool IsPointerOverBlockedUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            GameObject go = result.gameObject;

            // ⛔️ InputField, Button 은 허용
            if (go.GetComponent<UnityEngine.UI.Button>() != null ||
                go.GetComponent<TMPro.TMP_InputField>() != null)
            {
                return false;
            }
        }

        return results.Count > 0; // 다른 UI에 닿았으면 true
    }
}