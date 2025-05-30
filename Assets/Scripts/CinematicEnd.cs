using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CinematicEnd : MonoBehaviour
{
    public PlayableDirector director;
    void Start()
    {
        // Ÿ�Ӷ��� ����� ������ �̺�Ʈ ȣ��
        director.stopped += OnTimelineFinished;
    }
    void OnTimelineFinished(PlayableDirector pd)
    {
        // ���� ������ �̵�
        SceneManager.LoadScene("1stFloor");
    }
}
