using System.ComponentModel;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/Item")]
public class ItemData : ScriptableObject
{
    //아이템 명
    public string itemName;

    //아이템 아이콘->인벤 슬롯에 보이는 이미지
    public Sprite icon;

    //아이템 프리팹
    public GameObject prefab;

    //아이템 구분
    public bool isStoryItem;
}
