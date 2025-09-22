using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class UIMon : MonoBehaviour
{
    [SerializeField] private Vector3 targetTransform1, targetTransform2;
    [SerializeField] private PlayableDirector timeLine;
    [SerializeField] private GameObject uiCam;
    private CinemachineBasicMultiChannelPerlin noise;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        noise = uiCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Start()
    {
        StartCoroutine(Teleport(2f, targetTransform1));
        StartCoroutine(TeleportWithAnim(4f, targetTransform2));
    }
    
    private IEnumerator Teleport(float waitTime, Vector3 targetTransform)
    {
        yield return new WaitForSeconds(waitTime);
        transform.position = targetTransform;
    }

    private IEnumerator TeleportWithAnim(float waitTime, Vector3 targetTransform)
    {
        yield return new WaitForSeconds(waitTime);
        transform.position = targetTransform;
        animator.SetTrigger("Teleport");
    }
    // 헤딩 애니메이션 종료 후 이벤트 메소드
    public void OnAniFinished()
    {
        Debug.Log("AniFinished");
        timeLine.Play();
    }

    public void OnShake()
    {
        StartCoroutine(DoShake(3f, 5f, 0.7f));
    }

    private IEnumerator DoShake(float amplitude, float frequency, float duration)
    {
        noise.AmplitudeGain = amplitude;
        noise.FrequencyGain = frequency;
        yield return new WaitForSeconds(duration);
        noise.FrequencyGain = 0f;
        noise.AmplitudeGain = 0f;
    }
}