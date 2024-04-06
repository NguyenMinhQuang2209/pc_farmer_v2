using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceController : MonoBehaviour
{
    public static PreferenceController instance;

    public static string PLAYER_TAG = "Player";

    [HideInInspector] public Transform player;
    [SerializeField] private List<NestItem> nestItems = new();
    [SerializeField] private List<Pet> petItems = new();
    [SerializeField] private List<Item> items = new();
    [SerializeField] private List<DataSaveItem> worldItems = new();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
    }
    public Transform GetPlayer()
    {
        return GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
    }
    public Item GetItem(string itemName)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];
            if (item.GetItemName() == itemName)
            {
                return item;
            }
        }
        return null;
    }
    public Transform GetNest(NestName name)
    {
        for (int i = 0; i < nestItems.Count; i++)
        {
            NestItem nestItem = nestItems[i];
            if (nestItem.nestName == name)
            {
                return nestItem.nest;
            }
        }
        return null;
    }

    public Item GetItem(ItemName itemName)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];
            if (item.GetName() == itemName)
            {
                return item;
            }
        }
        return null;
    }

    public DataSaveItem GetWorldItem(string itemName)
    {
        for (int i = 0; i < worldItems.Count; i++)
        {
            DataSaveItem item = worldItems[i];
            if (item.GetItemName().ToString() == itemName)
            {
                return item;
            }
        }
        return null;
    }

    public DataSaveItem GetWorldItem(WorldItemName itemName)
    {
        for (int i = 0; i < worldItems.Count; i++)
        {
            DataSaveItem item = worldItems[i];
            if (item.GetItemName() == itemName)
            {
                return item;
            }
        }
        return null;
    }

    public Pet GetPet(string petName)
    {
        for (int i = 0; i < petItems.Count; i++)
        {
            Pet item = petItems[i];
            if (item.GetPetName().ToString() == petName)
            {
                return item;
            }
        }
        return null;
    }

    public Pet GetPet(PetName petName)
    {
        for (int i = 0; i < petItems.Count; i++)
        {
            Pet item = petItems[i];
            if (item.GetPetName() == petName)
            {
                return item;
            }
        }
        return null;
    }
}
[System.Serializable]
public class NestItem
{
    public NestName nestName;
    public Transform nest;
}