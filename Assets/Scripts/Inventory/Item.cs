using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [Header ("아이템 데이터")]
    public ItemData ItemData;


    //래이캐스트 프롬띄우기
    public string GetPromptText()
    {
        return "줍기";
    }

    public void Interact()
    {
        InventoryItem item = new InventoryItem(ItemData, gameObject);
        Debug.Log($"Interact 호출됨: {ItemData.itemName}");
        if (ItemData.isStoryItem)
        {
            //스토리용 아이템일때
            Inventory.Instance.AddInventory(item);
        }
        else
        {
            //상호작용용 아이템일때
            if (!Inventory.Instance.isHandEmty)
            {
                Debug.Log("손에 아이템이 있습니다");
                return;
            }            
            Inventory.Instance.AddHand(item);
        }
        //아이템 비활성화
        gameObject.SetActive(false);
        Debug.Log(item.data.itemName + "획득");
    }

}
