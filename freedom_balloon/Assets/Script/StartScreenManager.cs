using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class StartScreenManager : MonoBehaviour
{
    public GameObject startCanvas;
    public GameObject gameplayCanvas;
    public GameManager gameManager;

    private bool gameStarted = false;

    void Start()
    {
        gameManager.enabled = false; // 게임 로직 비활성화로 시작
    }

    void Update()
    {
        // 게임이 이미 시작되었거나, 이름 입력 중이면 완전 차단
        if (gameStarted || GameManager.isInNameInputMode)
        {
            Debug.Log("⛔ 이름 입력 중 or 게임 시작됨 → 입력 차단");
            return;
        }

        if (IsValidClick())
        {
            StartGame();
        }
    }

    bool IsValidClick()
    {
#if UNITY_EDITOR
        return Input.GetMouseButtonDown(0) && !IsPointerOverAnyUI();
#else
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began &&
               !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
    }

    bool IsPointerOverAnyUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results.Count > 0;
    }

    void StartGame()
    {
        Debug.Log("🎮 게임 시작!");
        startCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);
        gameManager.enabled = true;
        gameStarted = true;
    }
}