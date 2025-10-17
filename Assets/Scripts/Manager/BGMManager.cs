using System.Collections;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioSource bgmSource;
    public AudioClip[] startBGM;
    public AudioClip[] livingRoomBGM;
    public AudioClip[] MiddleBGM;
    public AudioClip[] EndingCorriderBGM;

    private Coroutine currentPlaylistCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환에도 유지되도록
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBGMPlaylist(startBGM);
    }

    // 🎵 단일 클립 재생 (기존 유지)
    public void PlayBGM(AudioClip clip)
    {
        StopCurrentPlaylist(); // 중복 방지
        if (clip == null) return;

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.Play();
    }
    // 🎼 여러 클립 순차 재생
    public void PlayBGMPlaylist(AudioClip[] clips)
    {
        StopCurrentPlaylist();

        if (clips == null || clips.Length == 0)
            return;

        currentPlaylistCoroutine = StartCoroutine(PlayPlaylistCoroutine(clips));
    }
    private IEnumerator PlayPlaylistCoroutine(AudioClip[] clips)
    {
        foreach (var clip in clips)
        {
            bgmSource.clip = clip;
            bgmSource.Play();

            // 현재 클립이 끝날 때까지 대기
            while (bgmSource.isPlaying)
            {
                yield return null;
            }

            // 약간 딜레이를 줄 수도 있음
            yield return new WaitForSeconds(0.2f);
        }

        currentPlaylistCoroutine = null;
    }

    private void StopCurrentPlaylist()
    {
        if (currentPlaylistCoroutine != null)
        {
            StopCoroutine(currentPlaylistCoroutine);
            currentPlaylistCoroutine = null;
        }
    }
}