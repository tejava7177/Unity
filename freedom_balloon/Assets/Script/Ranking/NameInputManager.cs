using System.Collections;
using UnityEngine;
using TMPro;

public class NameInputManager : MonoBehaviour
{
    public GameObject inputPanel;
    public TMP_InputField nameInputField;

    public void Show()
    {
        inputPanel.SetActive(true);
        nameInputField.text = "";
        nameInputField.ActivateInputField();
        SetInputMode(true);
    }

    public void Hide()
    {
        inputPanel.SetActive(false);
        SetInputMode(false);
    }

    public void OnConfirmButtonClick()
    {
        string nickname = nameInputField.text;

        if (string.IsNullOrWhiteSpace(nickname))
        {
            Debug.LogWarning("닉네임이 비어있습니다!");
            return;
        }

        RankingManager.instance.AddNewScore(nickname, GameManager.instance.GetScore());

        Hide();

        // 필요 시, 메인 화면 이동 등 처리
    }

    private void SetInputMode(bool isActive)
    {
        GameManager.isInNameInputMode = isActive;
        GameManager.instance.enabled = !isActive;
    }
}