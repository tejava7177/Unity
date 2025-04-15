using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    // 트리거 충돌 시 호출됨
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 풍선이 데드존에 닿았을 때 제거
        if (other.CompareTag("Balloon"))
        {
            Destroy(other.gameObject);
            Debug.Log("💥 풍선이 데드존에 닿아 제거됨");
        }
    }
}