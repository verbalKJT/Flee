using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //싱글톤으로 구현
    public static Inventory Instance { get; private set; }

    //인벤토리 리스트
    public List <InventoryItem> inventory;
    //아이템 슬롯 UI 배열(5칸)
    public Image[] ItemSlotUI=new Image[5];    

    //손에 드는 아이템
    public InventoryItem hand;
    public bool isHandEmty=true;

    void Awake()
    {
        if (Instance == null) Instance = this;

        inventory = new List<InventoryItem>();//리스트 초기화
    }

    public void Update()
    {
        if (!isHandEmty && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log(CrosshairInteractor.hitPoint);
            UseHandItem(hand);
        }
    }

    //스토리용 아이템 획득
    public void AddInventory(InventoryItem item)
    {
        inventory.Add(item);
        Debug.Log(item.data.itemName + "획득");
        //슬롯에 이미지 띄우기
        ItemSlotUpdate();
    }

    //아이템 슬롯 업데이트
    public void ItemSlotUpdate()
    {
        //
        int index = 0;

        foreach (var item in inventory)
        {
            if (!item.data.isStoryItem) continue;
            if (index >= ItemSlotUI.Length) break;

            ItemSlotUI[index].sprite = item.data.icon;
            ItemSlotUI[index].color = Color.white;
            index++;
        }
    }

    //인벤토리아이템 비우기
    public void DelItem(InventoryItem item)
    {
        //리스트에서 삭제
        inventory.Remove(item);
        Debug.Log(item.data.itemName + "가 삭제됨");
    }

    //아이템 손에 들기 
    public void AddHand(InventoryItem item)
    {   
       hand = item;       
        Debug.Log(item.data.itemName + " 손에들기");
        isHandEmty = false;
        //손위치에 아이템 활성화
    }

    //손에있는 아이템 사용
    public void UseHandItem(InventoryItem item)
    {
        
        Debug.Log(item.data.itemName + "아이템 사용");

        //래이캐스트 hit위치에 아이템 활성화        
        item.instance.transform.position = CrosshairInteractor.hitPoint;
        item.instance.gameObject.SetActive(true);

        Debug.Log(CrosshairInteractor.hitPoint+"에 "+item.data.itemName+"활성화 ");

        //손 비움
        isHandEmty = true;
        
        
    }   

    
}
