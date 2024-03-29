using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactible
{
    [SerializeField] private Item item;

    public override void Interact()
    {
        bool canPickup = InventoryController.instance.PickupItem(item);
        if (canPickup)
        {
            Destroy(gameObject);
        }
    }
}
[System.Serializable]
public class Item
{
    public Sprite sprite;
    public ItemName itemName;
    [SerializeField] private int currentQuantity = 1;
    [SerializeField] private int maxQuantity = 1;

    public bool UseStack()
    {
        return maxQuantity > 1;
    }
    public int GetCurrentQuantity()
    {
        return currentQuantity;
    }
    public string GetItemName()
    {
        return itemName.ToString();
    }

    public int Add(int value)
    {
        int next = currentQuantity + value;
        if (next > maxQuantity)
        {
            currentQuantity = maxQuantity;
            return next - maxQuantity;
        }
        currentQuantity = next;
        return 0;
    }
    public void ChangeItemQuantity(int next)
    {
        currentQuantity = next;
    }
}
