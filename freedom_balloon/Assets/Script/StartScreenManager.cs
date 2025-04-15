using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class StartScreenManager : MonoBehaviour
{
    public GameObject startCanvas;     // 시작 화면 UI
    public GameObject gameplayCanvas;  // 게임 중 UI
    public GameManager gameManager;    // GameManager 참조

    private bool gameStarted = false;  // 게임 시작 여부

    void Start()
    {
        // 게임 시작 전에는 GameManager 비활성화
        gameManager.enabled = false;
    }

    void Update()
    {
        // 이미 게임이 시작되었거나, 이름 입력 중이면 입력 차단
        if (gameStarted || GameManager.isInNameInputMode)
        {
            return;
        }

        // 유효한 입력 시 게임 시작
        if (IsValidClick())
        {
            StartGame();
        }
    }

    // 플랫폼별 입력 처리 및 UI 클릭 여부 확인
    bool IsValidClick()
    {
#if UNITY_EDITOR
        return Input.GetMouseButtonDown(0) && !IsPointerOverAnyUI();
#else
        return Input.touchCount > 0 &&
               Input.GetTouch(0).phase == TouchPhase.Began &&
               !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
    }

    // 클릭 위치가 UI 요소 위인지 확인
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

    // 게임 시작 처리
    void StartGame()
    {
        startCanvas.SetActive(false);     // 시작 화면 숨김
        gameplayCanvas.SetActive(true);   // 게임 UI 표시
        gameManager.enabled = true;       // 게임 로직 활성화
        gameStarted = true;
    }
}