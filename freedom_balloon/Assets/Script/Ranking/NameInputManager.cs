using System.Collections;
using UnityEngine;
using TMPro;

// 게임 오버 후 이름을 입력받고 점수를 저장하는 역할
public class NameInputManager : MonoBehaviour
{
    public GameObject inputPanel;            // 이름 입력 UI 패널
    public TMP_InputField nameInputField;    // TMP 입력 필드

    // 입력 패널을 보여주고 입력 준비 상태로 전환
    public void Show()
    {
        inputPanel.SetActive(true);
        nameInputField.text = "";
        nameInputField.ActivateInputField();
        SetInputMode(true);
    }

    // 입력 패널을 숨기고 게임을 다시 조작 가능 상태로 전환
    public void Hide()
    {
        inputPanel.SetActive(false);
        SetInputMode(false);
    }

    // Enter 버튼 클릭 시 호출됨
    public void OnConfirmButtonClick()
    {
        string nickname = nameInputField.text;

        if (string.IsNullOrWhiteSpace(nickname))
        {
            Debug.LogWarning("닉네임이 비어있습니다!");
            return;
        }

        // 점수와 이름을 랭킹에 저장
        RankingManager.instance.AddNewScore(nickname, GameManager.instance.GetScore());

        Hide(); // 입력 종료
    }

    // 입력 중일 때 게임 로직 일시 정지
    private void SetInputMode(bool isActive)
    {
        GameManager.isInNameInputMode = isActive;
        GameManager.instance.enabled = !isActive;
    }
}