using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ThrownSoundEmitter : MonoBehaviour
{
    public float soundRange = 15f;
    private bool hasEmitted = false;

    void OnCollisionEnter(Collision collision)
    {
        if (hasEmitted) return;

        hasEmitted = true;

        // 몬스터 AI에게 전달
        SoundManager.EmitSound(transform.position, soundRange);

        // 소리 재생 (선택)
        // var audio = GetComponent<AudioSource>();
        // if (audio != null)
        // {
        //     audio.Play();
        // }

        // 3초 후 제거 (사운드 끝날 때까지)
        // Destroy(gameObject, 3f);
    }
}