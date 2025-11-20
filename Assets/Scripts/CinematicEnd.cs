using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CinematicEnd : MonoBehaviour
{
    public PlayableDirector director;

    private bool hasSkipped = false;
    void Start()
    {
        // 타임라인 종료 시 씬 전환
        director.stopped += OnTimelineFinished;
    }
    void Update()
    {
        if (hasSkipped) return;

        // VR 컨트롤러 버튼 (예: A 버튼, Start, 트리거 등) 아무거나
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("🎮 VR 컨트롤러 입력으로 스킵");
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