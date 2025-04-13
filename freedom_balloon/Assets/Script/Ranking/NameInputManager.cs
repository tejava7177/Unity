using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class NameInputManager : MonoBehaviour
{
    public GameObject inputPanel; // 이 패널 전체
    public TMP_InputField nameInputField; // 입력창

    public void Show()
    {
        inputPanel.SetActive(true);
    }

    public void OnConfirmButtonClick()
    {
        string nickname = nameInputField.text;

        if (string.IsNullOrEmpty(nickname))
        {
            Debug.LogWarning("닉네임이 비어있습니다!");
            return;
        }

        int score = GameManager.instance.GetScore(); // GameManager에서 현재 점수 가져오기
        RankingManager.instance.AddNewScore(nickname, score); // 랭킹에 저장
        Debug.Log($"✅ 저장 완료: {nickname} - {score}");

        inputPanel.SetActive(false); // 입력 패널 숨김

        // 🎯 나중엔 랭킹 화면으로 이동해도 좋아!
    }
}