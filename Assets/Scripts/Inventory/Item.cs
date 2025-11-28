using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [Header("������ ������")] public ItemData ItemData;
    private IInteractable _interactableImplementation;


    //����ĳ��Ʈ ���Ҷ���
    public string GetPromptText()
    {
        return "�ݱ�";
    }

    // VR 손과 트리거 충돌 시 호출되는 함수
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 "VRHand" 태그를 가지고 있는지 확인합니다.
        // 이 태그는 VR 손의 콜라이더가 있는 오브젝트에 직접 설정되어야 합니다.
        if (other.CompareTag("RightController"))
        {
            // VR 손과 닿았을 때만 Interact 로직을 실행합니다.
            // VR 에서는 마우스 클릭이 아니므로 IInteractable 인터페이스의 Interact() 구현은 
            // VRHand 태그가 있는 오브젝트와의 충돌로 대체됩니다.
            HandlePickup();
        }
    }

    // 아이템 획득 로직을 별도의 함수로 분리 (기존 Interact()의 내용)
    // OnTriggerEnter에서 이 함수를 호출하도록 변경합니다.
    private void HandlePickup()
    {
        InventoryItem item = new InventoryItem(ItemData, gameObject);
        Debug.Log($"자동 획득 호출: {ItemData.itemName}");

        // 스토리 아이템만 자동 획득되도록 요청하셨으므로,
        // isStoryItem이 true일 때만 획득 로직을 실행하고, 일반 아이템 획득 로직은 삭제합니다.
        if (ItemData.isStoryItem)
        {
            // 스토리 아이템은 바로 인벤토리에 추가
            Inventory.Instance.AddInventory(item);

            // 획득 후 오브젝트 비활성화
            gameObject.SetActive(false);
            Debug.Log(item.data.itemName + " 획득 (스토리 아이템)");
        }
        else
        {
            Debug.Log("이것은 상호작용(Interaction)이 필요한 일반 아이템입니다.");
        }
    }
     public void Interact()
    {
        InventoryItem item = new InventoryItem(ItemData, gameObject);
        Debug.Log($"Interact ȣ���: {ItemData.itemName}");
        if (ItemData.isStoryItem)
        {
            //���丮�� �������϶�
            Inventory.Instance.AddInventory(item);
        }
        else
        {
            //��ȣ�ۿ�� �������϶�
            if (!Inventory.Instance.isHandEmty)
            {
                Debug.Log("�տ� �������� �ֽ��ϴ�");
                return;
            }
            Inventory.Instance.AddHand(item);
        }
        //������ ��Ȱ��ȭ
        gameObject.SetActive(false);
        Debug.Log(item.data.itemName + "ȹ��");
    } 
}
