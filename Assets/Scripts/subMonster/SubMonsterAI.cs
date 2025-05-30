using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SubMonsterAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 10f;  
    public float stopDistance = 1.5f;          // 정지 거리
    public AudioClip foundPlayerSound;         // 사운드 클립
    private AudioSource audioSource;           // 오디오 소스
    private bool hasPlayedSound = false;       // 중복 방지
    private NavMeshAgent agent;
    private Animator animator;

    private bool hasCaughtPlayer = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (hasCaughtPlayer)
        {
            // 정지 상태 처음
            agent.isStopped = true;
            agent.ResetPath();
            animator.SetFloat("Speed", 0f);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetFloat("Speed", agent.velocity.magnitude);

            // 플레이어 처음 발견했을 때 효과음
            if (!hasPlayedSound && foundPlayerSound != null)
            {
                audioSource.PlayOneShot(foundPlayerSound);
                hasPlayedSound = true;
            }

            if (distance <= stopDistance)
            {
                StartCoroutine(CatchPlayerAndStop()); // 딱 붙잡히면 정지 + 블라인드
            }
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
            animator.SetFloat("Speed", 0f);
        }
    }

    IEnumerator CatchPlayerAndStop()
    {
        hasCaughtPlayer = true;

        // 한번 잡고 그 자리에서 멈춤
        agent.ResetPath();
        agent.isStopped = true;
        animator.SetFloat("Speed", 0f);

        // 시야 블라인드
        PlayerVision vision = player.GetComponent<PlayerVision>();
        if (vision != null)
        {
            yield return vision.BlindForSeconds(5f); // 5초동안 암전
        }
    }
}