using UnityEngine;

public class StoryDoorLock : MonoBehaviour
{
    [Header("문을 열기위한 스토리아이템 개수")]
    public int ItemCount;

    [Header("벽난로기믹수행 체크해야하는 문")]
    public bool CheckFirepalce;

    public bool IsPossibleOpen()
    {
        if (CheckFirepalce)
        {
            if (Inventory.Instance.MonInFireplace)
                return true;
            return false;

        }
        else
        {
            return CheckItemCount();
        }
    }
    public bool CheckItemCount()
    {
        bool possibleOpen;
            //현재 플레이어가 가지고 있는 스토리용 아이템 개수
        int CurrentItemCount= Inventory.Instance.CheckAllStoryItemsCollected();

        if (Inventory.Instance.MonInFireplace)
        {

        }

            //필요한 아이템개수와 비교 후 문 동작 가능여부 반환
        if (CurrentItemCount >= ItemCount)
        {//현재 소지 아이템개수가 필요아이템개수보다 많으면 문 동작 가능
            possibleOpen = true;
        }
        else
        {
            possibleOpen = false;
        }

        return possibleOpen;
    }

 
}
