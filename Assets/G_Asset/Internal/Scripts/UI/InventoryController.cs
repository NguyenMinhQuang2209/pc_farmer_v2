using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController instance;

    public Transform drag_parent;

    [SerializeField] private Transform trash_store;
    [SerializeField] private InventoryItem inventory_item;
    [SerializeField] private GameObject ui_container;
    [SerializeField] private GameObject b_inventory;
    [SerializeField] private GameObject ui_chest;
    [SerializeField] private GameObject ui_shop;

    [Header("Inventory Setup")]
    [SerializeField] private int currentInventorySlot = 10;
    [SerializeField] private int maxSlot = 20;
    [SerializeField] private InventorySlot slot;
    [SerializeField] private Transform inventory_container;

    [Header("Quick slot")]
    [SerializeField] private int currentQuickSlot = 10;
    [SerializeField] private int maxQuickSlot = 10;
    [SerializeField] private Transform quickslot_container;

    [Header("Chest UI")]
    [SerializeField] private int currentChestSlot = 1;
    [SerializeField] private int maxChestSlot = 16;
    [SerializeField] private Transform chestslot_container;

    [Header("Shop UI")]
    [SerializeField] private int maxShopSlot = 16;
    [SerializeField] private Transform shopslot_container;

    private Dictionary<int, InventorySlot> inventorySlotStores = new();
    private Dictionary<int, InventorySlot> quickSlotStores = new();
    private Dictionary<int, InventorySlot> chestSlotStores = new();
    private Dictionary<int, InventorySlot> shopSlotStores = new();

    private Chest currentChest = null;

    private List<InventoryItem> twoSideChestItems = new();

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
        ui_chest.SetActive(true);
        ui_shop.SetActive(true);

        SpawnItem(currentInventorySlot, maxSlot, inventory_container, false, inventorySlotStores, false);
        SpawnItem(currentQuickSlot, maxQuickSlot, quickslot_container, true, quickSlotStores, false);
        SpawnItem(currentChestSlot, maxChestSlot, chestslot_container, false, chestSlotStores, false);
        SpawnItem(maxShopSlot, maxShopSlot, shopslot_container, false, shopSlotStores, true);

        for (int i = 0; i < maxChestSlot; i++)
        {
            twoSideChestItems.Add(null);
        }

        ui_container.SetActive(false);
        b_inventory.SetActive(false);
        ui_chest.SetActive(false);
        ui_shop.SetActive(false);
    }

    public void SpawnItem(int current, int max, Transform spawnPosition, bool showQuickslot, Dictionary<int, InventorySlot> store, bool isShop)
    {
        foreach (Transform child in spawnPosition)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < max; i++)
        {
            InventorySlot tempSlot = Instantiate(slot, spawnPosition.transform);
            tempSlot.InventorySlotInit(showQuickslot ? (i + 1).ToString() : "", isShop);
            store[i] = tempSlot;
            if (i >= current)
            {
                tempSlot.gameObject.SetActive(false);
            }
        }
    }
    public int PickupItem(Item item, int quantity)
    {
        ItemInit itemInit = new(item.sprite, item.itemName, item.currentQuantity, item.maxQuantity, item.price, item.buyRate);
        for (int i = 0; i < currentInventorySlot; i++)
        {
            InventorySlot currentSlot = inventorySlotStores[i];
            if (currentSlot.ExistItem())
            {
                ItemInit currentItem = currentSlot.GetInventoryItem();
                if (itemInit.GetItemName() == currentItem.GetItemName())
                {
                    int remain = currentItem.Add(quantity);
                    if (remain == 0)
                    {
                        return 0;
                    }
                    quantity = remain;
                }
            }
            else
            {
                int nextQuantity = quantity < itemInit.GetMaxQuantity() ? quantity : itemInit.GetMaxQuantity();
                InventoryItem tempItem = Instantiate(inventory_item, currentSlot.GetItemContainer().transform);
                tempItem.InventoryItemInit(item, nextQuantity, false);
                quantity -= nextQuantity;
                if (quantity == 0)
                {
                    return 0;
                }
            }
        }
        return quantity;
    }

    public void InteractWithInventory()
    {
        CursorController.instance.ChangeCursor("Inventory", new() { ui_container, b_inventory });
    }
    public void InteractWithChest(Chest current)
    {
        if (currentChest != current)
        {
            InteractWithSlot(current, current.GetCurrentSlot(), maxChestSlot, chestSlotStores);
        }
        CursorController.instance.ChangeCursor("Interact_Chest", new() { ui_container, b_inventory, ui_chest });
    }

    public void InteractWithSlot(Chest current, int slot, int maxSlot, Dictionary<int, InventorySlot> stores)
    {
        if (currentChest == current)
        {
            return;
        }
        if (currentChest != null)
        {
            if (currentChest.OneSideChest())
            {
                currentChest.SetListItems(GetChestListItem(currentChest.GetCurrentSlot()));
            }
            else
            {
                StoringTwoSideChest(currentChest.GetCurrentSlot());
            }
        }
        currentChest = current;
        slot = Mathf.Min(slot, maxSlot);
        List<InventoryItem> listItems = currentChest.OneSideChest() ? currentChest.GetListItems() : twoSideChestItems;
        for (int i = 0; i < maxSlot; i++)
        {
            InventorySlot currentSlot = stores[i];
            currentSlot.gameObject.SetActive(i < slot);
            if (listItems.Count > i)
            {
                InventoryItem tempItem = listItems[i];
                if (tempItem != null)
                {
                    tempItem.gameObject.SetActive(true);
                    tempItem.transform.SetParent(currentSlot.GetItemContainer());
                }
            }
        }
    }
    public List<InventoryItem> GetChestListItem(int slot)
    {
        List<InventoryItem> list = new();
        for (int i = 0; i < slot; i++)
        {
            InventorySlot current_slot = chestSlotStores[i];
            InventoryItem currentItem = current_slot.GetInventory();
            if (currentItem != null)
            {
                currentItem.transform.SetParent(trash_store.transform);
                currentItem.gameObject.SetActive(false);
            }
            list.Add(currentItem);
        }
        return list;
    }

    public void StoringTwoSideChest(int slot)
    {
        for (int i = 0; i < slot; i++)
        {
            InventorySlot current_slot = chestSlotStores[i];
            InventoryItem currentItem = current_slot.GetInventory();
            if (currentItem != null)
            {
                currentItem.transform.SetParent(trash_store.transform);
                currentItem.gameObject.SetActive(false);
            }
            twoSideChestItems[i] = currentItem;
        }
    }
}
