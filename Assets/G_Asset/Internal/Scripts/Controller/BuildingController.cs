using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public static BuildingController instance;
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
        CursorController.instance.ChangeCursor("", new() { });
        Transform player = PreferenceController.instance.player;
        if (player.gameObject.TryGetComponent<PlayerBuilding>(out var playerBuilding))
        {
            playerBuilding.ChangeBuildingItem(buildingItem);
        }
    }
}
