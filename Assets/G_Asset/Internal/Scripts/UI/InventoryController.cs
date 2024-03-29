using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController instance;

    public Transform drag_parent;

    [SerializeField] private GameObject ui_container;
    [SerializeField] private GameObject b_inventory;

    [Header("Inventory Setup")]
    [SerializeField] private int currentInventorySlot = 10;
    [SerializeField] private int maxSlot = 20;
    [SerializeField] private InventorySlot slot;
    [SerializeField] private Transform inventory_container;

    [Header("Quick slot")]
    [SerializeField] private int currentQuickSlot = 10;
    [SerializeField] private int maxQuickSlot = 10;
    [SerializeField] private Transform quickslot_container;

    [SerializeField] private InventoryItem inventory_item;

    private Dictionary<int, InventorySlot> inventorySlotStores = new();
    private Dictionary<int, InventorySlot> quickSlotStores = new();

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
        ui_container.SetActive(true);
        b_inventory.SetActive(true);

        SpawnItem(currentInventorySlot, maxSlot, inventory_container, false, inventorySlotStores);
        SpawnItem(currentQuickSlot, maxQuickSlot, quickslot_container, true, quickSlotStores);

        ui_container.SetActive(false);
        b_inventory.SetActive(false);
    }

    public void SpawnItem(int current, int max, Transform spawnPosition, bool show, Dictionary<int, InventorySlot> store)
    {
        foreach (Transform child in spawnPosition)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < max; i++)
        {
            InventorySlot tempSlot = Instantiate(slot, spawnPosition.transform);
            tempSlot.InventorySlotInit(show ? (i + 1).ToString() : "");
            store[i] = tempSlot;
            if (i >= current)
            {
                tempSlot.gameObject.SetActive(false);
            }
        }
    }
    public bool PickupItem(Item item)
    {
        for (int i = 0; i < currentInventorySlot; i++)
        {
            InventorySlot currentSlot = inventorySlotStores[i];
            if (currentSlot.ExistItem())
            {
                if (item.GetItemName() == currentSlot.GetItemName())
                {
                    Debug.Log("Got item");
                }
            }
            else
            {
                InventoryItem tempItem = Instantiate(inventory_item, currentSlot.GetItemContainer().transform);
                tempItem.InventoryItemInit(item);
                return true;
            }
        }
        return false;
    }

    public void InteractWithInventory()
    {
        CursorController.instance.ChangeCursor("Inventory", new() { ui_container, b_inventory });
    }
}
