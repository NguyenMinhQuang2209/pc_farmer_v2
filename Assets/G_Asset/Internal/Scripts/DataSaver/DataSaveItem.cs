using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaveItem : MonoBehaviour
{
    [SerializeField] private WorldItemName itemName;
    public WorldItemName GetItemName()
    {
        return itemName;
    }
}
