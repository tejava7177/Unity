using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject thirdPersonCam;
    public GameObject firstPersonCam;
    public GameObject wideViewCam;

    private int cameraIndex = 0; // 0: 3인칭, 1: 1인칭, 2: 전체 뷰

    void Start()
    {
        SetCameraView(cameraIndex); // 시작은 3인칭
    }

    public void SwitchCamera()
    {
        cameraIndex++;
        if (cameraIndex > 2)
        {
            cameraIndex = 0; // 다시 0부터 순환
        }
        SetCameraView(cameraIndex);
    }

    private void SetCameraView(int index)
    {
        thirdPersonCam.SetActive(index == 0);
        firstPersonCam.SetActive(index == 1);
        wideViewCam.SetActive(index == 2);
    }
}