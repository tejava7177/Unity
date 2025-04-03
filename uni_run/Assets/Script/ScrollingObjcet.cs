using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    public float speed = 10f;       //이동속도

    void Update()
    {
        if (!GameManager.instance.isGameover)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);     //초당 speed 속도로 평행이동
        }
    }
}
