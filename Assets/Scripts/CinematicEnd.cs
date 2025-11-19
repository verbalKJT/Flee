using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CinematicEnd : MonoBehaviour
{
    public PlayableDirector director;

    private bool hasSkipped = false;
    private bool canSkip = false; // ⛔ 시작 직후엔 입력 막기
    private float skipDelay = 5f; // 🎬 5초 후 스킵 가능
    
    void Start()
    {
        // 타임라인 종료 시 씬 전환
        director.stopped += OnTimelineFinished;
        
        // ⏳ 일정 시간 뒤부터 입력 허용
        Invoke(nameof(EnableSkip), skipDelay);
    }
    void EnableSkip()
    {
        canSkip = true;
        Debug.Log("✅ 이제 스킵 가능");
    }
    void Update()
    {
        if (!canSkip || hasSkipped) return;

        // VR 컨트롤러 버튼 (예: A 버튼, Start, 트리거 등) 아무거나
        if (OVRInput.GetDown(OVRInput.Button.One) ||
            OVRInput.GetDown(OVRInput.Button.Two) ||
            OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Debug.Log("🎮 VR 컨트롤러 입력으로 스킵");
            SkipCinematic();
        }

        // 키보드 입력도 허용 (엔터, 스페이스 등)
        if (Input.anyKeyDown)
        {
            Debug.Log("⌨️ 키보드 입력으로 스킵");
            SkipCinematic();
        }
    }

    // 타임라인이 자연스럽게 끝났을 때
    void OnTimelineFinished(PlayableDirector pd)
    {
        if (!hasSkipped)
        {
            LoadNextScene();
        }
    }
    
    // 스킵 버튼 클릭 시   
    public void SkipCinematic()
    {
        if (hasSkipped) return;
        hasSkipped = true;

        // 타임라인 중지
        director.Stop();

        // 바로 다음 씬으로 이동
        LoadNextScene();
    }

    // 씬 이동 코드 분리
    private void LoadNextScene()
    {
        SceneManager.LoadScene("1stFloor");
    }
}