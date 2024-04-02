using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite sprite;
    public ItemName itemName;
    public int currentQuantity = 1;
    public int maxQuantity = 1;
    public int price = 1;
    public float buyRate = 1;
}
[System.Serializable]
public class ItemInit
{
    public Sprite sprite;
    public ItemName itemName;
    [SerializeField] private int currentQuantity = 1;
    [SerializeField] private int maxQuantity = 1;
    [SerializeField] private int price = 1;
    [SerializeField] private float buyRate = 1;

    public ItemInit(Sprite sprite, ItemName itemName, int currentQuantity, int maxQuantity, int price, float buyRate)
    {
        this.sprite = sprite;
        this.itemName = itemName;
        this.currentQuantity = currentQuantity;
        this.maxQuantity = maxQuantity;
        this.price = price;
        this.buyRate = buyRate;
    }
    public bool UseStack()
    {
        return maxQuantity > 1;
    }
    public bool CanAdd()
    {
        return UseStack() && currentQuantity < maxQuantity;
    }
    public int GetCurrentQuantity()
    {
        return currentQuantity;
    }
    public string GetItemName()
    {
        return itemName.ToString();
    }
    public int GetPricePerUnit()
    {
        return price;
    }
    public int GetBuyPricePerUnit()
    {
        return (int)Mathf.Ceil(buyRate * price);
    }
    public int GetTotalPrice()
    {
        return price * currentQuantity;
    }
    public int GetMaxQuantity()
    {
        return maxQuantity;
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

    public ItemInit Clone()
    {
        return new(sprite, itemName, currentQuantity, maxQuantity, price, buyRate);
    }

}