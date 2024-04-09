using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseBuildingItem : Use_Item
{
    [SerializeField] private BuildingItem buildingItem;
    public override void UseItem(InventoryItem item = null)
    {
        BuildingController.instance.UseBuildingItem(buildingItem);
    }
}
