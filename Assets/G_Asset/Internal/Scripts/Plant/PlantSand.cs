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

            promptMessage = Mathf.Floor(targetTime - currentGrowingTime).ToString() + "s";

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
                Debug.Log("Collecting");
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
        plant = newPlant;
        currentIndex = 1;
        targetTime = plant.GetGrowingTime();
        periodTime = plant.TotalPeriodPerTime();
        currentGrowingTime = 0f;
        canCollect = false;
        ChangeSprite(0);
    }

    public void ChangeSprite(int pos)
    {
        Sprite newSprite = plant.GetSprite(pos);
        spriteRenderer.sprite = newSprite;
    }
}
