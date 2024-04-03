using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite sprite;
    public ItemName itemName;
    public int maxQuantity = 1;
    public int price = 1;
    public float buyRate = 1;
    public void UseItem()
    {
        if (TryGetComponent<Use_Item>(out var useItem))
        {
            useItem.UseItem();
        }
    }

    public bool UseStack()
    {
        return maxQuantity > 1;
    }

    public string GetItemName()
    {
        return itemName.ToString();
    }
    public int GetSellPricePerUnit()
    {
        return price;
    }
    public int GetBuyPricePerUnit()
    {
        return (int)Mathf.Ceil(buyRate * price);
    }

    public int GetMaxQuantity()
    {
        return maxQuantity;
    }
    /*public int Add(int value)
    {
        int next = currentQuantity + value;
        if (next > maxQuantity)
        {
            currentQuantity = maxQuantity;
            return next - maxQuantity;
        }
        currentQuantity = next;
        return 0;
    }*/
}
