using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [Header("발소리 설정")]
    public AudioSource audioSource;       // 캐릭터에 붙어 있는 AudioSource

    [Header("걷기 발소리")]
    public AudioClip[] MainMonFootstepClips;     // 발소리 여러 개 넣어두면 랜덤 재생
    public AudioClip[] MiddleMonFootstepClips;
    public AudioClip[] SubMonFootstepClips;   // ★ 추가
    public AudioClip[] CrawlMonFootstepClips;   // ★ 추가

    [Header("달리기 발소리")]
    public AudioClip[] MainMonRunFootstepClips;
    public AudioClip[] MiddleMonRunFootstepClips;
    public AudioClip[] SubMonRunFootstepClips; // ★ 추가
    public AudioClip[] CrawlMonRunFootstepClips; // ★ 추가

    //메인 몬스터와 미들몬스터 발소리를 다르게 재생할 수 있도록 각각 배열 생성

    private AudioClip[] playingFootstepClips; //각각 실제 재생할 오디오 클립
    
    //볼륨 권장 범위는 0~1, 1이상 넘어가면 깨지는 소리 들릴 수도 있다고 함
    [Range(0.0f, 2.0f)]
    public float walkvolume = 0.8f;

    [Range(0.0f, 2.0f)]
    public float runVolume = 1.3f;
    //매번 똑같은 발소리가 아니라 약간 다른 소리가 들리도록,
    //발소리 클립에 피치를 조정하여 소리를 높거나 낮게 함.
    [Range(0.8f, 1.2f)]
    public float pitchRandomRange = 0.05f;   // 피치 살짝 랜덤

    // ★ Animation Event 에서 호출할 함수
    //걷을 때 소리를 재생할 함수
    public void PlayFootstep()
    {
        //태그를 비교해 재생할 발소리 오디오 클립을 playingFootstepClips에 저장
        if (this.CompareTag("MainMon"))
        {
            playingFootstepClips = MainMonFootstepClips;
        }
        else if (this.CompareTag("MiddleMon"))
        {
            playingFootstepClips = MiddleMonFootstepClips;
        }
        else if (this.CompareTag("SubMon"))
        {
            playingFootstepClips = SubMonFootstepClips;  // 이 배열 새로 추가 필요
        }
        else if (this.CompareTag("CrawlMon"))
        {
            playingFootstepClips = CrawlMonFootstepClips;  // 이 배열 새로 추가 필요
        }
        else
            playingFootstepClips = null;

        PlayFromArray(playingFootstepClips, walkvolume);
    }

    // --- 달리기 ---
    public void PlayRunFootstep()
    {
        if (CompareTag("MainMon"))
            playingFootstepClips = MainMonRunFootstepClips;
        else if (CompareTag("MiddleMon"))
            playingFootstepClips = MiddleMonRunFootstepClips;
        else if (CompareTag("SubMon"))
            playingFootstepClips = SubMonRunFootstepClips;
        else if (CompareTag("CrawlMon"))
            playingFootstepClips = CrawlMonRunFootstepClips;
        else
            playingFootstepClips = null;

        PlayFromArray(playingFootstepClips, runVolume);
    }

    private void PlayFromArray(AudioClip[] clips, float volumeScale)
    {
        if (audioSource == null || clips == null || clips.Length == 0)
            return;

        int index = Random.Range(0, clips.Length);
        AudioClip clip = clips[index];

        float randomPitch = 1f + Random.Range(-pitchRandomRange, pitchRandomRange);
        audioSource.pitch = randomPitch;

        // AudioSource.volume(인스펙터 값)에 volumeScale 곱해서 최종 볼륨 결정
        audioSource.PlayOneShot(clip, volumeScale);
    }
}
