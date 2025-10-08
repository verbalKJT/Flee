using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    protected Transform player;   // 플레이어 Transform 참조
    
    public void SetupPlayerTransform(Transform playerTransform)
    {
        this.player = playerTransform;
    }
    public abstract void OnPlayerSetupComplete();
}
