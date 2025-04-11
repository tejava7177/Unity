using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchManager : MonoBehaviour
{
    public GameObject blueBalloonPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(worldPos);

            if (col != null && col.CompareTag("Balloon"))
            {
                Vector3 pos = col.transform.position;

                // 기존 노란 풍선 제거
                Destroy(col.gameObject);

                // 파란 풍선 생성 → Balloon.cs 안에서 알아서 떠오르게 됨!
                Instantiate(blueBalloonPrefab, pos, Quaternion.identity);
            }
        }
    }
}