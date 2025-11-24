using UnityEngine;

public class MonsterSpawnTrigger : MonoBehaviour, IInteractable
{
    

    public void Interact()
    {
        Debug.Log("작업실 몬스터 스폰!");
    }

    public string GetPromptText()
    {
        return "소환";
    }
}
