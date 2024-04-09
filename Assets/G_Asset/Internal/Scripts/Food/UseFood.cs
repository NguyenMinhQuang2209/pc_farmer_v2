using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseFood : Use_Item
{
    [SerializeField] private float value = 10f;
    [SerializeField] private float plusDuration = 0f;
    [SerializeField] private float duractionEffectTimer = 0f;


    public override void UseItem(InventoryItem inventoryItem)
    {
        if (inventoryItem != null)
        {
            FoodController.instance.UseFoodItem(inventoryItem, value, plusDuration, duractionEffectTimer);
        }
    }

    public Vector3 GetValue()
    {
        return new(value, plusDuration, duractionEffectTimer);
    }

}
