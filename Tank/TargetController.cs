using UnityEngine;

public class TargetController : MonoBehaviour
{
    public enum TargetType { Destructible, Solid }
    public TargetType targetType;

    public ParticleSystem explosionEffect;  // 폭발 효과
    public ParticleSystem hitEffect;        // 일반 충돌 효과

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("SHELL")) return;

        Vector3 hitPos = collision.contacts[0].point;

        switch (targetType)
        {
            case TargetType.Destructible:
                if (explosionEffect != null)
                {
                    ParticleSystem effect = Instantiate(explosionEffect, hitPos, Quaternion.identity);
                    effect.Play();
                    Destroy(effect.gameObject, 2f);
                }
                Destroy(gameObject); // 파괴 가능한 오브젝트 제거
                break;

            case TargetType.Solid:
                if (hitEffect != null)
                {
                    ParticleSystem effect = Instantiate(hitEffect, hitPos, Quaternion.identity);
                    effect.Play();
                    //Destroy(effect.gameObject, 2f);
                }
                // Solid는 파괴되지 않음
                break;
        }
    }
}