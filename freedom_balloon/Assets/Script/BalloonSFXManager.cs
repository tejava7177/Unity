using UnityEngine;

public class BalloonSFXManager : MonoBehaviour
{
    public static BalloonSFXManager instance;

    public AudioClip popClip;
    private AudioSource[] audioSources;
    private int currentIndex = 0;

    public int poolSize = 5;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 풀링된 오디오소스 생성
        audioSources = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject audioObj = new GameObject("BalloonSFX_" + i);
            audioObj.transform.parent = this.transform;

            AudioSource source = audioObj.AddComponent<AudioSource>();
            audioSources[i] = source;
        }
    }

    public void PlayPopSound(Vector3 position)
    {
        AudioSource source = audioSources[currentIndex];
        source.transform.position = position;
        source.PlayOneShot(popClip);

        currentIndex = (currentIndex + 1) % audioSources.Length;
    }
}