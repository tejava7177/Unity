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
        // 게임이 진행 가능한 상태가 아니면 입력 무시
        if (!GameManager.instance.IsGamePlayable())
            return;

        // 클릭/터치 입력 처리
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverBlockedUI())
                return;

            HandleBalloonTouch();
        }
    }

    // 풍선 터치 처리
    void HandleBalloonTouch()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D col = Physics2D.OverlapPoint(worldPos);

        if (col == null) return;

        if (!col.CompareTag("Balloon")) return;

        Balloon balloon = col.GetComponent<Balloon>();
        if (balloon == null) return;

        if (balloon.isLaunching)
            return;

        // 보너스 풍선 처리
        switch (balloon.balloonType)
        {
            case BalloonType.TimeBonus:
                GameManager.instance.AddTime(5f);
                balloon.Pop();
                return;

            case BalloonType.ScoreBonus:
                GameManager.instance.ActivateDoubleScore();
                balloon.Pop();
                return;

            case BalloonType.SlowEffect:
                GameManager.instance.ApplySlowEffect();
                balloon.Pop();
                return;
        }

        // 일반 풍선 진화 처리
        Vector3 pos = balloon.transform.position;

        switch (balloon.balloonColor)
        {
            case Balloon.BalloonColor.Yellow:
                Instantiate(blueBalloonPrefab, pos, Quaternion.identity);
                GameManager.instance.AddScore(10);
                break;

            case Balloon.BalloonColor.Blue:
                Instantiate(purpleBalloonPrefab, pos, Quaternion.identity);
                GameManager.instance.AddScore(20);
                break;

            case Balloon.BalloonColor.Purple:
                GameManager.instance.AddScore(30);
                break;
        }

        balloon.Pop(); // 효과음과 함께 제거
    }

    // UI 클릭 방지 (Button, InputField 제외)
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

            // Button, InputField는 예외로 허용
            if (go.GetComponent<Button>() != null ||
                go.GetComponent<TMP_InputField>() != null)
                return false;
        }

        // UI 요소에 닿은 경우
        return results.Count > 0;
    }

    // 외부에서 풍선 터치 시뮬레이션 (예: 필살기)
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

        //Destroy(balloon.gameObject);
        balloon.Pop();
    }
}