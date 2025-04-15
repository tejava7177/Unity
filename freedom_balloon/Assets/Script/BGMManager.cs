using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    private AudioSource bgm;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        bgm = GetComponent<AudioSource>();

        if (bgm != null)
        {
            bgm.time = 4f;  // ⏩ 2초 지점부터 재생
            bgm.Play();
        }
    }
}