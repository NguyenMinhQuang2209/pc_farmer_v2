using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaverController : MonoBehaviour
{
    public static DataSaverController instance;
    public static string DATA_SAVER_TAG = "Store";
    public static string DATA_SAVER_PET_TAG = "Pet";
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {

        }
    }
    public List<WorldItemDataStore> GetWorldItemData()
    {
        List<WorldItemDataStore> worldItemsData = new();
        GameObject[] datas = GameObject.FindGameObjectsWithTag(DATA_SAVER_TAG);
        for (int i = 0; i < datas.Length; i++)
        {
            GameObject item = datas[i];
            if (item.TryGetComponent<DataSaveItem>(out var dataItem))
            {
                WorldItemDataStore tempItem = new();

                WorldItemName itemName = dataItem.GetItemName();

                tempItem.worldItemName = itemName.ToString();
                tempItem.posX = item.transform.position.x;
                tempItem.posY = item.transform.position.y;
                tempItem.posZ = item.transform.position.z;

                List<InventoryItemDataStore> list = new();
                string plantName = "";
                if (item.TryGetComponent<Chest>(out var chest))
                {
                    List<InventoryItem> items = chest.GetListItems();
                    for (int j = 0; j < items.Count; j++)
                    {
                        InventoryItem inventoryItem = items[j];
                        if (inventoryItem != null)
                        {
                            InventoryItemDataStore tempInventoryItem = new();

                            tempInventoryItem.itemName = inventoryItem.GetItemName();
                            tempInventoryItem.inSlot = j;
                            tempInventoryItem.currentQuantity = inventoryItem.GetCurrentQuantity();

                            list.Add(tempInventoryItem);
                        }
                    }
                }
                else if (item.TryGetComponent<PlantSand>(out var plant))
                {
                    plantName = plant.PlantName();
                }
                tempItem.storeItems = list;
                tempItem.plantItem = plantName;
            }
        }
        return worldItemsData;
    }

    public PlayerDataStore GetPlayerData()
    {
        PlayerDataStore playerData = new();
        Transform player = PreferenceController.instance.GetPlayer();
        if (player != null && player.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerData.posX = player.position.x;
            playerData.posY = player.position.y;
            playerData.posZ = player.position.z;

            playerData.currentLevel = LevelController.instance.GetCurrentLevel();
            playerData.plusHealth = LevelController.instance.GetPlusHealth();
            playerData.plusFood = LevelController.instance.GetPlusFood();
            playerData.plusSpeed = LevelController.instance.GetPlusSpeed();
            playerData.plusRecover = LevelController.instance.GetPlusRecover();

            playerData.currentExe = LevelController.instance.GetCurrentExe();
            playerData.currentHealth = playerHealth.GetCurrentHealth();
            playerData.currentFood = playerHealth.GetCurrentFood();
        }
        return playerData;
    }
    public List<PetDataStore> GetPetsData()
    {
        List<PetDataStore> petsData = new();
        GameObject[] pets = GameObject.FindGameObjectsWithTag(DATA_SAVER_PET_TAG);
        for (int i = 0; i < pets.Length; i++)
        {
            GameObject currentPet = pets[i];
            PetDataStore petData = new();
            if (currentPet.TryGetComponent<Pet>(out var pet))
            {
                petData.petName = pet.GetPetName().ToString();
                petData.currentLevel = pet.GetCurrentLevel();
                petData.currentHealth = pet.GetCurrentHealth();
                petData.currentFood = pet.GetCurrentFood();
                petData.currentExe = pet.GetCurrentExe();
                petData.currentState = pet.GetCurrentMode().ToString();
            }
            petsData.Add(petData);
        }
        return petsData;
    }

    public List<InventoryItemDataStore> GetInventoryItems()
    {
        return GetListItems(InventoryController.instance.GetInventorySlots());
    }

    public List<InventoryItemDataStore> GetQuickslotItems()
    {
        return GetListItems(InventoryController.instance.GetQuickSlots());
    }

    public List<InventoryItemDataStore> GetListItems(List<InventorySlot> slots)
    {
        List<InventoryItemDataStore> list = new();
        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlot currentSlot = slots[i];
            InventoryItem inventoryItem = currentSlot.GetInventory();
            if (inventoryItem != null)
            {
                InventoryItemDataStore tempItem = new();

                tempItem.itemName = inventoryItem.GetItemName();
                tempItem.inSlot = i;
                tempItem.currentQuantity = inventoryItem.GetCurrentQuantity();

                list.Add(tempItem);
            }
        }
        return list;
    }
}
[System.Serializable]
public class WorldItemDataStore
{
    public string worldItemName = "";
    public int slot = 1;
    public float posX = 0;
    public float posY = 0;
    public float posZ = 0;
    public List<InventoryItemDataStore> storeItems = new();
    public string plantItem = "";
}
[System.Serializable]
public class InventoryItemDataStore
{
    public string itemName = "";
    public int inSlot = 1;
    public int currentQuantity = 0;
}
[System.Serializable]
public class PlayerDataStore
{
    public float posX = 0;
    public float posY = 0;
    public float posZ = 0;
    public float currentExe = 0;
    public int currentLevel = 0;
    public int plusHealth = 0;
    public int plusFood = 0;
    public int plusSpeed = 0;
    public int plusRecover = 0;
    public float currentHealth = 0f;
    public float currentFood = 0f;
}
[System.Serializable]
public class PetDataStore
{
    public string petName = "";
    public int currentLevel = 0;
    public string currentState = "";
    public float currentExe = 0;
    public float currentHealth = 0f;
    public float currentFood = 0f;
}