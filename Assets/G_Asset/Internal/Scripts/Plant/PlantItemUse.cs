using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantItemUse : Use_Item
{
    [SerializeField] private PlantItem plantItem;
    public override void UseItem()
    {
        PlantController.instance.ChangePlantItem(plantItem);
    }
}