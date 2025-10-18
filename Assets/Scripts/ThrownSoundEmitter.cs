using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ThrownSoundEmitter : MonoBehaviour
{
    public float soundRange = 15f;
    private bool hasEmitted = false;
    public AudioClip emitSound;
    public bool isThrown = false;

    void OnCollisionEnter(Collision collision)
    {
        if (!isThrown || hasEmitted) return;

        hasEmitted = true;

        // 몬스터 AI에게 전달
        SoundManager.EmitSound(transform.position, soundRange);

        // 소리 재생 추가
        var audio = GetComponent<AudioSource>();
        if (audio != null && emitSound != null)
        {
            audio.clip = emitSound;
            audio.Play();
        }

        // 3초 후 제거 
        Destroy(gameObject, 3f);
    }
}