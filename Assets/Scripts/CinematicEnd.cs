using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CinematicEnd : MonoBehaviour
{
    public PlayableDirector director;
    void Start()
    {
        // 타임라인 재생이 끝나면 이벤트 호출
        director.stopped += OnTimelineFinished;
    }
    void OnTimelineFinished(PlayableDirector pd)
    {
        // 다음 씬으로 이동
        SceneManager.LoadScene("1stFloor");
    }
}
