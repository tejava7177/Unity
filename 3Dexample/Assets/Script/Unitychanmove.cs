using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Unitychanmove : MonoBehaviour
{
    float h, v;
    float speed = 3.0f;

    bool jumping;
    float lastTime;

    Animator mAvatar;

    public void OnTouchValueChanged(Vector2 stickPos){
        h = stickPos.x;
        v = stickPos.y;
    }

    public void Start(){
        mAvatar = GetComponent<Animator>();
    }


    void Update(){
        mAvatar.SetFloat("Speed", (h * h + v * v));

        if(h != 0f && v != 0f){
            transform.Rotate(0, h, 0);
            transform.Translate(0, 0, v*speed * Time.deltaTime);
        }
    }


    public void OnJumpBtnDown(){
        jumping = true;

        StartCoroutine(JumpHero());
    }

    public void OnJumpBtnUp(){
        jumping = false;
    }

    IEnumerator JumpHero(){
        if(Time.time - lastTime > 1.0f){
            lastTime = Time.time;
            while(jumping){
                mAvatar.SetTrigger("Jump");

                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
