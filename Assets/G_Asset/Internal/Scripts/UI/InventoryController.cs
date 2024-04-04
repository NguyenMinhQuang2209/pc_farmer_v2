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


    private Dictionary<string, int> quantityStores = new();
    private Dictionary<ItemType, int> typeQuantityStores = new();

    private Chest currentChest = null;
    private Shop currentShop = null;


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

    private void Update()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyUp(KeyCode.Alpha0 + i))
            {
                ClickQuickslot(i - 1);
            }
        }

    }
    public void ClickQuickslot(int index)
    {
        if (currentQuickSlot >= index)
        {
            InventorySlot slot = quickSlotStores[index];
            InventoryItem inventoryItem = slot.GetInventory();
            if (inventoryItem != null)
            {
                inventoryItem.UseItem();
            }
        }
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
        int remain = PickupItemInventory(item, quantity);
        ReloadQuantityItem();
        return remain;
    }
    private int PickupItemInventory(Item item, int quantity)
    {
        for (int i = 0; i < currentInventorySlot; i++)
        {
            InventorySlot currentSlot = inventorySlotStores[i];
            if (currentSlot.ExistItem())
            {
                InventoryItem inventoryItem = currentSlot.GetInventory();
                Item currentItem = currentSlot.GetInventoryItem();
                if (item.GetItemName() == currentItem.GetItemName())
                {
                    int remain = inventoryItem.Add(quantity);
                    if (remain == 0)
                    {
                        return 0;
                    }
                    quantity = remain;
                }
            }
            else
            {
                int nextQuantity = quantity < item.GetMaxQuantity() ? quantity : item.GetMaxQuantity();
                InventoryItem tempItem = Instantiate(inventory_item, currentSlot.GetItemContainer().transform);
                tempItem.InventoryItemInit(item, nextQuantity, false);
                quantity -= nextQuantity;
                if (quantity <= 0)
                {
                    return 0;
                }
            }
        }
        if (quantity > 0)
        {
            LogController.instance.Log(LogMode.Inventory_Full);
        }
        return quantity;
    }


    public void InteractWithInventory()
    {
        ReloadQuantityItem();
        CursorController.instance.ChangeCursor("Inventory", new() { ui_container, b_inventory });
    }
    public void InteractWithChest(Chest current)
    {
        ReloadQuantityItem();
        if (currentChest != current)
        {
            InteractWithSlot(current, current.GetCurrentSlot(), maxChestSlot, chestSlotStores);
        }
        CursorController.instance.ChangeCursor("Interact_Chest", new() { ui_container, b_inventory, ui_chest });
    }
    public int GetQuantity(string itemName)
    {
        return quantityStores.ContainsKey(itemName) ? quantityStores[itemName] : 0;
    }
    public bool EnoughToRemove(string itemName, int quantity)
    {
        int remain = quantityStores.ContainsKey(itemName) ? quantityStores[itemName] : 0;
        return remain >= quantity;
    }
    private int RemoveItemInventory(string itemName, int quantity)
    {
        for (int i = 0; i < currentInventorySlot; i++)
        {
            InventorySlot currentSlot = inventorySlotStores[i];
            InventoryItem inventoryItem = currentSlot.GetInventory();
            if (inventoryItem != null)
            {
                if (inventoryItem.GetItemName() == itemName)
                {
                    int remain = inventoryItem.Minus(quantity);
                    inventoryItem.CheckQuantity();
                    quantity = remain;
                    if (quantity == 0)
                    {
                        return 0;
                    }
                }
            }
        }
        for (int i = 0; i < currentQuickSlot; i++)
        {
            InventorySlot currentSlot = quickSlotStores[i];
            InventoryItem inventoryItem = currentSlot.GetInventory();
            if (inventoryItem != null)
            {
                if (inventoryItem.GetItemName() == itemName)
                {
                    int remain = inventoryItem.Minus(quantity);
                    inventoryItem.CheckQuantity();
                    quantity = remain;
                    if (quantity == 0)
                    {
                        return 0;
                    }
                }
            }
        }
        return quantity;
    }
    public int RemoveItem(string itemName, int quantity)
    {
        int remain = RemoveItemInventory(itemName, quantity);
        ReloadQuantityItem();
        return remain;
    }

    public void InteractWithShop(Shop shop)
    {
        ReloadQuantityItem();
        if (currentShop != shop)
        {
            InteractWithSlotShop(shop, maxShopSlot, shopSlotStores);
        }
        CursorController.instance.ChangeCursor("Interact_Shop", new() { ui_container, b_inventory, ui_shop });
    }

    public List<InventoryItem> InventoryShopInitItem(List<Item> items)
    {
        List<InventoryItem> tempItems = new();
        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];
            InventoryItem tempItem = Instantiate(inventory_item, trash_store.transform);
            tempItem.InventoryItemInit(item, 1, true);
            tempItems.Add(tempItem);
        }
        return tempItems;
    }

    public List<InventoryItem> InventoryInitItem(List<PickupItemDetail> items, bool isShop = false)
    {
        List<InventoryItem> tempItems = new();
        for (int i = 0; i < items.Count; i++)
        {
            PickupItemDetail item = items[i];
            if (item.useRandom)
            {
                int min = Mathf.Min(item.quantityRange.x, item.quantityRange.y);
                int max = Mathf.Max(item.quantityRange.x, item.quantityRange.y);
                int quan = Random.Range(min, max + 1);
                item.quantity = quan;
            }
            InventoryItem tempItem = Instantiate(inventory_item, trash_store.transform);
            int nextQuantity = item.quantity <= item.item.GetMaxQuantity() ? item.quantity : item.item.GetMaxQuantity();
            tempItem.InventoryItemInit(item.item, nextQuantity, isShop);
            tempItems.Add(tempItem);
        }
        return tempItems;
    }

    public void InteractWithSlotShop(Shop shop, int slot, Dictionary<int, InventorySlot> stores)
    {
        if (currentShop == shop)
        {
            return;
        }
        if (currentShop != null)
        {
            currentShop.SetListItems(GetChestListItem(maxShopSlot, shopSlotStores));
        }
        currentShop = shop;

        currentShop.InitInventory();

        List<InventoryItem> listItems = currentShop.GetListInventoryItems();
        for (int i = 0; i < slot; i++)
        {
            InventorySlot currentSlot = stores[i];
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

    public void ReloadQuantityItem()
    {
        quantityStores?.Clear();
        typeQuantityStores?.Clear();
        for (int i = 0; i < currentInventorySlot; i++)
        {
            InventorySlot currentSlot = inventorySlotStores[i];
            InventoryItem inventoryItem = currentSlot.GetInventory();
            if (inventoryItem != null)
            {
                ItemType type = inventoryItem.GetItem().GetItemType();
                int remain = quantityStores.ContainsKey(inventoryItem.GetItemName()) ? quantityStores[inventoryItem.GetItemName()] : 0;
                int remainType = typeQuantityStores.ContainsKey(type) ? typeQuantityStores[type] : 0;
                int next = remain + inventoryItem.GetCurrentQuantity();
                quantityStores[inventoryItem.GetItemName()] = next;
                typeQuantityStores[type] = remainType;
            }
        }
        for (int i = 0; i < currentQuickSlot; i++)
        {
            InventorySlot currentSlot = quickSlotStores[i];
            InventoryItem inventoryItem = currentSlot.GetInventory();
            if (inventoryItem != null)
            {
                ItemType type = inventoryItem.GetItem().GetItemType();
                int remain = quantityStores.ContainsKey(inventoryItem.GetItemName()) ? quantityStores[inventoryItem.GetItemName()] : 0;
                int remainType = typeQuantityStores.ContainsKey(type) ? typeQuantityStores[type] : 0;
                int next = remain + inventoryItem.GetCurrentQuantity();
                quantityStores[inventoryItem.GetItemName()] = next;
                typeQuantityStores[type] = remainType;
            }
        }
    }
    public int GetRemainTypeQuantity(ItemType type)
    {
        if (typeQuantityStores.ContainsKey(type))
        {
            return typeQuantityStores[type];
        }
        return 0;
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
                currentChest.SetListItems(GetChestListItem(currentChest.GetCurrentSlot(), chestSlotStores));
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
    public List<InventoryItem> GetChestListItem(int slot, Dictionary<int, InventorySlot> storings)
    {
        List<InventoryItem> list = new();
        for (int i = 0; i < slot; i++)
        {
            InventorySlot current_slot = storings[i];
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

    public void CollectingFullItem(Item item, int quantity, Vector3 pos)
    {
        int remain = PickupItem(item, quantity);
        if (remain > 0)
        {
            LogController.instance.Log(LogMode.Inventory_Full);
        }
    }
}
