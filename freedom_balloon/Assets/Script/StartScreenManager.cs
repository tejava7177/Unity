using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartScreenManager : MonoBehaviour
{
    public GameObject startCanvas; // UI 전체
    public GameObject gameplayCanvas; // Score/Timer 포함된 Canvas
    public GameManager gameManager;

    private bool gameStarted = false;

    void Update()
    {
        if (gameStarted) return;

        // 🎯 에디터 & 모바일 모두 대응
        bool userClicked = false;

#if UNITY_EDITOR
        userClicked = Input.GetMouseButtonDown(0);
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            userClicked = true;
#endif

        // 🎯 클릭 발생 + UI 위가 아닐 때만 게임 시작
        if (userClicked && !EventSystem.current.IsPointerOverGameObject(-1))
        {
            Debug.Log("🎮 게임 시작!");
            StartGame();
        }
    }

    void StartGame()
    {
        startCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);
        gameManager.enabled = true;
        gameStarted = true;
    }
}