using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    private float width;        //배경의 가로 길이
    private void Awake()
    {
        // boxCollider2D 컴포넌트의 Size 필드의 x 값을 가로길이로 사용
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        width = backgroundCollider.size.x;
    }


    // Update is called once per frame
    void Update()
    {
        //현재 위치가 원점에서 왼쪽으로 width 이상 이동했을 때 위치를 리셋
        if (transform.position.x <= -width)
        {
            Reposition();
        }
    }

    //위치를 리셋하는 메서드
    private void Reposition()
    {
        Vector2 offset = new Vector2(width * 2f, 0);        //현재 위치에서 오른쪽으로 가로길이 *2 만큼 이동
        transform.position = (Vector2)transform.position + offset;
    }
}
