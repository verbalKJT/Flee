using UnityEngine;
using System.Collections;
using System;

public class CeilingWaterTrigger : MonoBehaviour
{
    public GameObject waterOverlayObject;
    public float darkenDuration = 2f;
    public float stayDuration = 3f;
    public Transform teleportTarget;
    public GameObject player;

    public ResetBathroom roomResetTarget; // 초기화 대상 방
    public WaterManager waterManager;
    public RoomEntryTrigger RoomEntryTrigger;
    public SinkHandleManager SinkHandleManager;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Ceiling Trigger] 충돌 감지됨: {other.name}");
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("[Ceiling Trigger] 플레이어 충돌 감지됨");
            triggered = true;
            if (waterManager != null)
            {
                Debug.Log("[Ceiling Trigger] 물 상승 정지 호출");
                waterManager.StopRising();
                
            }
            StartCoroutine(HandleCeilingEvent());
        }
    }

    IEnumerator HandleCeilingEvent()
    {
        Debug.Log("[Ceiling Trigger] HandleCeilingEvent 시작");
        yield return new WaitForSeconds(stayDuration);

        /*화면 어두워짐
        CanvasGroup cg = waterOverlayObject?.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            waterOverlayObject.SetActive(true);
            float t = 0f;
            while (t < darkenDuration)
            {
                t += Time.deltaTime;
                cg.alpha = Mathf.Lerp(0f, 1f, t / darkenDuration);
                yield return null;
            }
            cg.alpha = 1f;
        }*/

        // 순간이동(CharacterController로 해야 순간이동 버그 없음)
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            Debug.Log("캐릭터");
            //잠시 없앤다음 순간위치로 이동후 생성
            cc.enabled = false;
            player.transform.position = teleportTarget.position;            
            cc.enabled = true;
            triggered = false;

            //물 에서 나옴
            waterManager.InWater = false;
            //문없어지는 트리거 초기화
            RoomEntryTrigger.triggered = false;
            //sink물 틀기 트리거 초기화
            SinkHandleManager.activated = false;
        }
        else
        {
            Debug.LogWarning("플레이어컨트롤러 할당 안됨");
        }

        // 방 상태 초기화
        if (roomResetTarget != null)
        {
            roomResetTarget.ResetRoom();
        }
    }

    public void setPlayer(GameObject player)
    {
        this.player = player;   
    }
}
