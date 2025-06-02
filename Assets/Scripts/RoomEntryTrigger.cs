using UnityEngine;

public class RoomEntryTrigger : MonoBehaviour
{
    [Header("��ȯ ���")]
    public GameObject doorObject;  // ���� �� ������Ʈ
    public GameObject wallObject;  // �� ��� ������ ��

    public bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (doorObject != null)
                doorObject.SetActive(false);

            if (wallObject != null)
                wallObject.SetActive(true);

            Debug.Log("�� ���� - �� ����� �� Ȱ��ȭ��");
        }
    }
}
