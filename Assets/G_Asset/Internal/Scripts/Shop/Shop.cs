using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : Interactible
{
    [SerializeField] private List<Item> items = new();
    private List<InventoryItem> inventoryItems = new();

    private bool init = false;
    public override void Interact()
    {
        InventoryController.instance.InteractWithShop(this);
    }
    public List<InventoryItem> GetListInventoryItems()
    {
        return inventoryItems;
    }
    public List<Item> GetItems()
    {
        return items;
    }
    public void InitInventory()
    {
        if (!init)
        {
            inventoryItems = InventoryController.instance.InventoryShopInitItem(items);
            init = true;
        }
    }
    public void SetListItems(List<InventoryItem> newItems)
    {
        inventoryItems = newItems;
    }
}
