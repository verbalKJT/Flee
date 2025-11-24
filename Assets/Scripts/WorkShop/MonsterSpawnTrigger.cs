using UnityEngine;

public class MonsterSpawnTrigger : MonoBehaviour, IInteractable
{
    

    public void Interact()
    {
        
        //스토리아이템 수집 완료 검사
        if (Inventory.Instance.hasAllStoryItems)
        {
            Debug.Log("아이템 6개 수집 완료"+ Inventory.Instance.hasAllStoryItems);
            Debug.Log("작업실 몬스터 스폰!");
        }
        else
        {
            Debug.Log("아이템 수집 개수가 부족합니다!"+Inventory.Instance.CheckAllStoryItemsCollected());
        }
    }

    public string GetPromptText()
    {
        return "소환";
    }
}
