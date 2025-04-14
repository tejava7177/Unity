using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ClockScript : MonoBehaviour
{
    bool btn_active;
    bool isPaused = false;
    public TextMeshProUGUI text_time;
    public TextMeshProUGUI btn_text;
    public float startTime;
    float timeRemaining;
    
    public float percent;

    public Texture2D clockBG;
    public Texture2D clockFG;
    public float clockFGMaxWidth;


    public Texture2D rightSide;
    public Texture2D leftSide;
    public Texture2D back;
    public Texture2D blocker;
    public Texture2D shiny;
    public Texture2D finished;


    string FormatTime(float time)
    {
        time = Mathf.Clamp(time, 0, Mathf.Infinity); // 음수 시간 방지
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    private void Start()
    {
        btn_active = false;
        isPaused = false;
        timeRemaining = startTime;
        clockFGMaxWidth = clockFG.width;

        text_time.text = FormatTime(timeRemaining); // <- 추가
        Debug.Log("Program start " + btn_active);
    }


    public void StartCustomTimer(float customTime)
    {
        startTime = customTime;
        timeRemaining = customTime;
        SetTimeOn();
        btn_text.text = "Stop"; // 기존 Start/Stop 버튼 상태 갱신
    }

    public void Btn_Click()
    {
        if (!btn_active)
        {
            timeRemaining = startTime;
            SetTimeOn();
            isPaused = false; // <-- 추가!
            btn_text.text = "Pause"; // 시작하면 일시정지 가능한 상태로
        }
        else
        {
            SetTimeOff();
            btn_text.text = "Start";
        }
    }

    public void SetTimeOn(){
        btn_active = true;
    }

    public void SetTimeOff(){
        btn_active = false;
    }



    private void Update() {
        if(btn_active && !isPaused){
            DoCountdown();
        }
    }


    void DoCountdown(){
        timeRemaining -= Time.deltaTime;
        text_time.text = FormatTime(timeRemaining);
        percent = timeRemaining / startTime * 100;
        if(timeRemaining < 0){
            timeRemaining = 0;
            btn_active = false;
            TimeIsUp();
        }
        Debug.Log("Time remaining = " + timeRemaining);
    }

    void TimeIsUp(){
        Debug.Log("Time is up!");
    }



    void OnGUI(){
        float newBarWidth = (percent / 100) * clockFGMaxWidth;
        int gap = 20;

        bool isPastHalfway = percent < 50;

        int pieClockX = 100;
        int pieClockY = 50;
        int pieClockW = 64;
        int pieClockH = 64;
        float pieClockHalfW = (float)(pieClockW * 0.5);
        float pieClockHalfH = (float)(pieClockH * 0.5);

        Rect clockRect = new Rect(pieClockX, pieClockY, pieClockW, pieClockH);

        


        float rot = (percent / 100) * 360;
        Vector2 centerPoint = new Vector2(pieClockX + pieClockHalfW, pieClockY+pieClockHalfH);
        Matrix4x4 startMartrix = GUI.matrix;




        GUI.DrawTexture(clockRect, back, ScaleMode.StretchToFill, true, 0);


        GUI.BeginGroup(new Rect(Screen.width - clockBG.width - gap, gap, clockBG.width, clockBG.height));
        GUI.DrawTexture(new Rect(0, 0, clockBG.width, clockBG.height), clockBG);
        GUI.BeginGroup(new Rect(5, 6, newBarWidth, clockFG.height));
        GUI.DrawTexture(new Rect(1, 0, clockFG.width, clockFG.height), clockFG);
        GUI.EndGroup();
        GUI.EndGroup();

        if (isPastHalfway){
            GUIUtility.RotateAroundPivot(-rot, centerPoint);
            GUI.DrawTexture(clockRect, leftSide, ScaleMode.StretchToFill, true, 0);
            GUI.matrix = startMartrix;
            GUI.DrawTexture(clockRect, blocker, ScaleMode.StretchToFill, true, 0);
        }
        else{
            GUIUtility.RotateAroundPivot(-rot+ 180, centerPoint);
            GUI.DrawTexture(clockRect, rightSide, ScaleMode.StretchToFill, true , 0);
            GUI.matrix = startMartrix;
            GUIUtility.RotateAroundPivot(180, centerPoint);
            GUI.DrawTexture(clockRect, leftSide, ScaleMode.StretchToFill, true, 0);
        }


        if(percent < 0){
            GUI.DrawTexture(clockRect, finished, ScaleMode.StretchToFill, true, 0);
        }
        GUI.DrawTexture(clockRect, shiny, ScaleMode.StretchToFill, true, 0);

        
    }



    public void TogglePause()
    {
        // 타이머가 진행 중일 때만 일시정지 가능
        if (!btn_active) return;

        if (!isPaused)
        {
            isPaused = true;
            //btn_active = false;
            btn_text.text = "Resume";
            Debug.Log("타이머 일시정지됨");
        }
        else
        {
            isPaused = false;
            btn_active = true;
            btn_text.text = "Pause";
            Debug.Log("타이머 재시작됨");
        }
    }


    public void ResetTimer()
    {
        btn_active = false;
        isPaused = false;
        timeRemaining = startTime;
        percent = 100;

        // 시간 텍스트 초기화 (예: MM:SS 형식)
        int minutes = Mathf.FloorToInt(startTime / 60);
        int seconds = Mathf.FloorToInt(startTime % 60);
        text_time.text = FormatTime(timeRemaining); // <- 간단하게 대체

        // Start/Stop 버튼 텍스트 초기화
        btn_text.text = "Start";

        Debug.Log("타이머 초기화 완료");
    }
}
