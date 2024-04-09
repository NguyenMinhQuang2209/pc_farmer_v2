using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveInteract : Interactible
{
    public string mapName;
    public MapMode mapMode;
    public Transform spawnPosition;
    private int caveIndex = 0;
    public string GetMode()
    {
        string returnValue = "Độ khó: ";
        switch (mapMode)
        {
            case MapMode.Easy:
                returnValue += "dễ";
                break;
            case MapMode.Medium:
                returnValue += "trung bình";
                break;
            case MapMode.Hard:
                returnValue += "khó";
                break;
            case MapMode.Nope:
                returnValue += "không có";
                break;
            default:
                returnValue += "không xác định";
                break;
        }
        return returnValue;
    }
    public override void Interact()
    {
        MapController.instance.InteractWithMap(caveIndex);
    }
    public Vector3 GetSpawnPosition()
    {
        return spawnPosition.position;
    }
    public void ChangeCaveIndex(int next)
    {
        caveIndex = next;
    }
}
