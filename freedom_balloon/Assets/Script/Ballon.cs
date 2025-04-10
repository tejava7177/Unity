using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon : MonoBehaviour
{
    public float swayAmplitude = 0.5f;   // 좌우 흔들림 범위
    public float swayFrequency = 2f;     // 흔들림 속도

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // 바람에 흔들리는 연출
        float sway = Mathf.Sin(Time.time * swayFrequency) * swayAmplitude;
        transform.position = new Vector3(startPos.x + sway, transform.position.y, transform.position.z);
    }
}
