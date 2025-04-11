using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 8f;
    public float moveableRange = 5.5f;

    public float power = 1000f;
    public GameObject cannonBall;
    public Transform spawnPoint;

    void Update(){
        float move = 0f;

#if UNITY_EDITOR
        move = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space)){
            Shoot();
        }
#else
        transform.Translate(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0, 0);
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -moveableRange, moveableRange), transform.position.y);
    
        if(Input.touchCount > 0){
            float screenWidth = Screen.width;
            for(int i =0; i< Input.touchCount; i++){
                Touch touch = Input.GetTouch(i);
                Vector2 touchPosition = touch.position;

                if(touch.phase == TouchPhase.Began)
                {
                    if(touchPosition.x < screenWidth / 3f){
                        move = -1f;
                    } else if (touchPosition.x > screenWidth * 2f / 3f){
                        move = 1f;
                    } else {
                        Shoot();
                    }
                }
            }
        }
#endif
        transform.Translate(move * speed * Time.deltaTime, 0, 0);

        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, -moveableRange, moveableRange),
            transform.position.y
        );
    
    }

    void Shoot(){
        GameObject newBullet = Instantiate(cannonBall, spawnPoint.position, Quaternion.identity) as GameObject;
        newBullet.GetComponent<Rigidbody2D>().AddForce(Vector3.up * power);
    }

}
