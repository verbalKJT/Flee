using UnityEngine;
using UnityEngine.Playables;

public class DeadTimeLine : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeline;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.name + "충돌");
            timeline.Play();  // 이 때 DeathCam이 Timeline에서 Live로 바뀜
        }
    }
}
