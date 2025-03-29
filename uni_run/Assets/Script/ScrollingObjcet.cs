using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObjcet : MonoBehaviour
{

    public float speed = 10f;
   
    // Update is called once per frame
    void Update()
    {
        if(!GameManager.instance.isGameover){
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        
    }
}
