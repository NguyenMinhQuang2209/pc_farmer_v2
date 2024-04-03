using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public static BuildingController instance;

    private BuildingItem currentItem = null;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void UseBuildingItem(BuildingItem buildingItem)
    {
        if (buildingItem != null)
        {
            PlantController.instance.ChangePlantItem(null);
        }
        currentItem = buildingItem;
        CursorController.instance.ChangeCursor("", new() { });
        Transform player = PreferenceController.instance.player;
        if (player.gameObject.TryGetComponent<PlayerBuilding>(out var playerBuilding))
        {
            playerBuilding.ChangeBuildingItem(buildingItem);
        }
    }
    public void RemoveBuildingItem()
    {
        if (currentItem == null)
        {
            return;
        }
        Transform player = PreferenceController.instance.player;
        if (player.gameObject.TryGetComponent<PlayerBuilding>(out var playerBuilding))
        {
            playerBuilding.ChangeBuildingItem(null);
        }
    }
}
