using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseBuildingItem : Use_Item
{
    [SerializeField] private BuildingItem buildingItem;
    public override void UseItem()
    {
        BuildingController.instance.UseBuildingItem(buildingItem);
    }
}
