using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private SpriteRenderer plantSpriteRender;
    private PlantItem currentPlantItem = null;
    private void Start()
    {
        plantSpriteRender.gameObject.SetActive(false);
    }

    public void ChangePlantItem(PlantItem newItem)
    {
        currentPlantItem = newItem;
        if (currentPlantItem != null)
        {
            plantSpriteRender.sprite = currentPlantItem.seedSprite;
        }
        plantSpriteRender.gameObject.SetActive(currentPlantItem != null);
    }
    public PlantItem UsePlantItem()
    {
        if (currentPlantItem != null)
        {
            InventoryController.instance.RemoveItem(currentPlantItem.itemName.ToString(), 1);
            int remain = InventoryController.instance.GetQuantity(currentPlantItem.itemName.ToString());
            PlantItem tempItem = currentPlantItem;
            if (remain == 0)
            {
                currentPlantItem = null;
                plantSpriteRender.gameObject.SetActive(false);
            }
            return tempItem;
        }
        return null;
    }
}
