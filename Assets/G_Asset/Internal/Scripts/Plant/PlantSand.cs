using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSand : Interactible
{
    private Plant plant = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    int currentIndex = 1;

    float targetTime = 0f;
    float periodTime = 0f;
    float currentGrowingTime = 0f;

    bool canCollect = false;
    string defaultPromptMessage = "";
    private void Start()
    {
        defaultPromptMessage = promptMessage;
    }

    private void Update()
    {
        if (canCollect)
        {
            return;
        }

        if (plant != null)
        {
            currentGrowingTime += Time.deltaTime;

            float currentPeriodTime = periodTime * currentIndex;

            promptMessage = Mathf.Ceil(targetTime - currentGrowingTime).ToString() + "s";

            if (currentPeriodTime <= currentGrowingTime)
            {
                currentIndex += 1;
                ChangeSprite(currentIndex - 1);
            }

            if (currentGrowingTime >= targetTime)
            {
                canCollect = true;
                promptMessage = "Có thể thu hoạch";
            }
        }
    }
    public override void Interact()
    {
        if (plant != null)
        {
            if (canCollect)
            {
                Item collectItem = plant.CollectItem();
                int collectQuantity = plant.CollectQuantity();
                InventoryController.instance.CollectingFullItem(collectItem, collectQuantity, transform.position);
                spriteRenderer.sprite = null;
                plant = null;
                promptMessage = defaultPromptMessage;
            }
        }
        else
        {
            PlantItem plantItem = PlantController.instance.UsePlantItem();
            if (plantItem != null)
            {
                ChangePlant(plantItem.plant);
            }
        }

    }
    public void ChangePlant(Plant newPlant)
    {
        canCollect = false;
        plant = newPlant;
        currentIndex = 1;
        currentGrowingTime = 0f;
        if (plant != null)
        {
            targetTime = plant.GetGrowingTime();
            periodTime = plant.TotalPeriodPerTime();
            ChangeSprite(0);
        }
    }
    public string PlantName()
    {
        if (plant != null)
        {
            return plant.GetItemName().ToString();
        }
        return "";
    }

    public void ChangeSprite(int pos)
    {
        Sprite newSprite = plant.GetSprite(pos);
        spriteRenderer.sprite = newSprite;
    }
}
