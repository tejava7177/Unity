using UnityEngine;

public class TankController : MonoBehaviour
{
    public float moveSpeed = 10f;     // 이동 속도
    public float rotSpeed = 150f;     // 회전 속도

    private Vector2 inputVector = Vector2.zero;

    // 터치 입력 전달받는 함수
    public void SetInput(Vector2 input)
    {
        inputVector = input;
    }

    void Update()
    {
        float moveAmount = inputVector.y * moveSpeed * Time.deltaTime;
        float rotAmount = inputVector.x * rotSpeed * Time.deltaTime;

        transform.Translate(Vector3.forward * moveAmount);
        transform.Rotate(Vector3.up * rotAmount);
    }
}