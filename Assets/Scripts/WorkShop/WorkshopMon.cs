using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class WorkshopMon : MonoBehaviour
{
  
    private Transform playerPosition;
    private Transform monsterPosition;

    private AudioSource audioSource;

    [Header("사운드 클립")]
    public AudioClip spawnClip;
    public AudioClip chaseClip;
    public AudioClip catchClip;
    public AudioClip fireplaceClip;

    private bool isChasing=false;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }
    private void Start()
    {
        // 싱글톤에서 정보 받아옴        
        playerPosition = WorkShopManager.Instance.playerResetPoint;
        monsterPosition = WorkShopManager.Instance.monsterResetPoint;

        //몬스터 생성 시 소리
        if (spawnClip != null)
            audioSource.PlayOneShot(spawnClip);
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CatchPlayer();

        Debug.Log($"[WorkshopMon] 충돌 감지됨: {other.name}");

        GameObject player = other.gameObject;

        if (player == null)
        {
            Debug.LogError("player가 null입니다.");
            return;
        }

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc == null)
        {
            Debug.LogError("CharacterController 못 찾음");
            return;
        }

        StartCoroutine(TeleportPlayer(player,cc));

        // 몬스터 순간이동
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
                transform.position = monsterPosition.position;
                agent.enabled = true;

                // 다시 플레이어 쫓기 시작
                agent.SetDestination(player.transform.position);
            }
        }

    IEnumerator TeleportPlayer(GameObject player, CharacterController cc)
    {
        cc.enabled = false;
        yield return null; // 한 프레임 대기
        yield return null; // 두 프레임 대기 (더 안정성 확보)

        player.transform.position = playerPosition.position;

        cc.enabled = true;
        Debug.Log("플레이어 순간이동 완료: " + player.transform.position);
    }

  
    public void StartChase()
    {
        if (chaseClip != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = chaseClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    public void StopChase()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.loop = false;
        }
    }

    public void CatchPlayer()
    {
        if (catchClip != null)
            audioSource.PlayOneShot(catchClip);

        // 잡기 연출 또는 애니메이션
    }

    public void EnterFireplace()
    {
        if (fireplaceClip != null)
            audioSource.PlayOneShot(fireplaceClip);

        // 연출 효과
    }

}



