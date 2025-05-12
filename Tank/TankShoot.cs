using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : MonoBehaviour
{
    public Rigidbody prefabShell;
    public Transform fireTransform;

    // Update 제거
    // Fire()를 public 으로 열어서 버튼에서 호출 가능하게 만듦
    public void Fire()
    {
        Debug.Log("🔥 Fire 버튼 클릭됨!");

        Rigidbody shellInstance = Instantiate(prefabShell, fireTransform.position, fireTransform.rotation) as Rigidbody;
        shellInstance.velocity = 20.0f * fireTransform.forward;
    }
}