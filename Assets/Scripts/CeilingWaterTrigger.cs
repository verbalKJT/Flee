using UnityEngine;
using System.Collections;

public class CeilingWaterTrigger : MonoBehaviour
{
    public GameObject waterOverlayObject;
    public float darkenDuration = 2f;
    public float stayDuration = 3f;
    public Transform teleportTarget;
    public GameObject player;

    public ResetBathroom roomResetTarget; // 초기화 대상 방
    public WaterManager waterManager;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            if (waterManager != null)
            {
                waterManager.StopRising();
                
            }
            StartCoroutine(HandleCeilingEvent());
        }
    }

    IEnumerator HandleCeilingEvent()
    {
        yield return new WaitForSeconds(stayDuration);

        // 화면 어두워짐
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
        }

        // 순간이동
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            player.transform.position = teleportTarget.position;
            cc.enabled = true;
        }

        // 방 상태 초기화
        if (roomResetTarget != null)
        {
            roomResetTarget.ResetRoom();
        }
    }
}
