using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    public static PlantController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void ChangePlantItem(PlantItem newItem)
    {
        if (newItem != null)
        {
            BuildingController.instance.UseBuildingItem(null);
        }
        Transform player = PreferenceController.instance.player;
        if (player.TryGetComponent<PlayerHand>(out var playerHand))
        {
            playerHand.ChangePlantItem(newItem);
        }
    }
    public PlantItem UsePlantItem()
    {
        Transform player = PreferenceController.instance.player;
        if (player.TryGetComponent<PlayerHand>(out var playerHand))
        {
            return playerHand.UsePlantItem();
        }
        return null;
    }
}
