using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public GameObject blueBalloonPrefab; // Inspector에서 연결

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(worldPos);

            if (col != null && col.CompareTag("Balloon"))
            {
                // 현재 위치 저장
                Vector3 pos = col.transform.position;

                // 기존 노란 풍선 제거
                Destroy(col.gameObject);

                // 파란 풍선 생성
                GameObject blueBalloon = Instantiate(blueBalloonPrefab, pos, Quaternion.identity);

                // 위로 올라가는 힘 주기
                Rigidbody2D rb = blueBalloon.GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * 300f, ForceMode2D.Impulse);
            }
        }
    }
}
