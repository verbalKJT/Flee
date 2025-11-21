using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [Header("발소리 설정")]
    public AudioSource audioSource;       // 캐릭터에 붙어 있는 AudioSource
    public AudioClip[] MainMonFootstepClips;     // 발소리 여러 개 넣어두면 랜덤 재생
    public AudioClip[] MiddleMonFootstepClips;
    //메인 몬스터와 미들몬스터 발소리를 다르게 재생할 수 있도록 각각 배열 생성

    private AudioClip[] playingFootstepClips; //각각 실제 재생할 오디오 클립

    [Range(0f, 1f)]
    public float volume = 0.7f;

    [Range(0.8f, 1.2f)]
    public float pitchRandomRange = 0.05f;   // 피치 살짝 랜덤

    // ★ Animation Event 에서 호출할 함수
    public void PlayFootstep()
    {
        //태그를 비교해 재생할 발소리 오디오 클립을 playingFootstepClips에 저장
        if(this.CompareTag("MainMon"))
        {
            playingFootstepClips = MainMonFootstepClips;
        }
        else if(this.CompareTag("MiddleMon"))
        {
            playingFootstepClips = MiddleMonFootstepClips;
        }

        if (audioSource == null || playingFootstepClips == null || playingFootstepClips.Length == 0)
            return;
        
        // 랜덤 클립 선택
        int index = Random.Range(0, playingFootstepClips.Length);
        AudioClip clip = playingFootstepClips[index];

        // 피치 약간 랜덤하게
        float randomPitch = 1f + Random.Range(-pitchRandomRange, pitchRandomRange);
        audioSource.pitch = randomPitch;

        audioSource.PlayOneShot(clip, volume);
    }
}
