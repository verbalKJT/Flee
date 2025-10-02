using UnityEngine;

public class InventoryItem
{
    public ItemData data;

    public GameObject instance;

    public InventoryItem(ItemData data, GameObject instance)
    {
        this.data = data;
        this.instance = instance;
    }
}
