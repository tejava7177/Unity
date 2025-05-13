using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Gizmos : MonoBehaviour
{
    public Color color1 = Color.yellow;
    public float Gizmo_Radius = 0.08f;

    public void OnDrawGizmos(){
        Gizmos.color = color1;
        Gizmos.DrawSphere(transform.position, Gizmo_Radius);
    }
}

