using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Balloon"))
        {
            Destroy(other.gameObject);
            Debug.Log("💥 풍선이 데드존에 닿아 제거됨");
        }
    }
}
