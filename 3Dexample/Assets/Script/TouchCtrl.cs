using System.Collections;
using System.Collections.Generic;
//using System.Numerics;

using UnityEngine;

public class TouchCtrl : MonoBehaviour
{

    public Unitychanmove player;
    RectTransform touchCtrl;

    private int touchID = -1;

    private Vector2 startPos = Vector2.zero;

    public float dragRadius = 50f;

    private bool btnPressed = false;

 
    void Start()
    {
       touchCtrl = GetComponent<RectTransform>();

       startPos = touchCtrl.position; 
    }


    public void TouchDown(){
        btnPressed = true;
    }


    public void TouchUp(){
        btnPressed = false;

        SendInputValue(startPos);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        HandheldTouchPhase();
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER || UNITY_STANDALONE_OSX
        SendInputValue(Input.mousePosition);
#endif
    }


    void HandheldTouchPhase(){
        int i = 0;

        if(Input.touchCount > 0){
            foreach(Touch touch in Input.touches){
                i++;

                Vector3 touchPos = new Vector3(touch.position.x, touch.position.y);

                switch(touch.phase){
                    case TouchPhase.Began:
                        if(touch.position.x <= (startPos.x + dragRadius)){
                            touchID = i;
                        }
                        break;

                    case TouchPhase.Moved:
                        if(touchID == i){
                            SendInputValue(touchPos);
                        }
                        break;
                    
                    case TouchPhase.Stationary:
                        if(touchID == i){
                            SendInputValue(touchPos);
                        }
                        break;

                    case TouchPhase.Ended:
                        if(touchID == i){
                            touchID = -1;
                        }
                        break;




                }
            }
        }
    }


    void SendInputValue(Vector2 inputPos){
        if(btnPressed){
            Vector2 gabPos = (inputPos - startPos);

            if(gabPos.sqrMagnitude <= dragRadius * dragRadius){
                touchCtrl.position = inputPos;
            }

            else{
                gabPos.Normalize();

                touchCtrl.position = startPos + gabPos * dragRadius;
            }
        }
        else{
            touchCtrl.position = startPos;
        }

        Vector2 touchPosXY = new Vector3(touchCtrl.position.x, touchCtrl.position.y);

        Vector2 diff = touchPosXY - startPos;
        Vector2 normDiff = new Vector2(diff.x/dragRadius, diff.y / dragRadius);

        if(player != null){
            player.OnTouchValueChanged(normDiff);
        }
    }
}
