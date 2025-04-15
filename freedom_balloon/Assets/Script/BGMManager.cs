using UnityEngine;

// 배경음악을 싱글톤으로 관리하는 매니저
public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    private AudioSource bgm;

    void Awake()
    {
        // 싱글톤 패턴 적용 (중복 제거)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않음
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        bgm = GetComponent<AudioSource>();

        if (bgm != null)
        {
            bgm.time = 4f;  // 시작 지점을 4초로 지정 (무음 구간 스킵)
            bgm.Play();     // BGM 재생
        }
    }
}