using UnityEngine;

public class MonsterSpawnTrigger : MonoBehaviour, IInteractable
{
    [Header ("작업실 맵")]
    public GameObject Map1;
    public GameObject Map2;

    //맵 변경 함수
    public void SwitchMap()
    {
        if (Map1.activeSelf)
        {//Map1이 활성화 상태라면 Map1은 비활성화 Map2는 활성화
            Map1.SetActive(false);
            Map2.SetActive(true);
            Debug.Log("맵 교체 : Map1 -> Map2");
        }
        else
        {
            Map1.SetActive(true);
            Map2.SetActive(false);
            Debug.Log("맵 교체 : Map2 -> Map1");
        }
           
    }
    public void Interact()
    {
        
        //스토리아이템 수집 완료 검사
        if (Inventory.Instance.hasAllStoryItems)
        {
            Debug.Log("아이템 6개 수집 완료"+ Inventory.Instance.hasAllStoryItems);
            Debug.Log("작업실 몬스터 스폰!");
            //맵 교체
            SwitchMap();           
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
