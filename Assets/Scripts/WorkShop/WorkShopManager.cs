using UnityEngine;

public class WorkShopManager : MonoBehaviour
{
    public static WorkShopManager Instance { get; private set; }

    [Header("순간이동 지점")]
    public Transform playerResetPoint;
    public Transform monsterResetPoint;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
