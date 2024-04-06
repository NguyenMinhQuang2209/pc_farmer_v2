using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{

    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI quickslotTxt;

    [SerializeField] private bool isShop = false;
    bool isInventorySlot = false;

    public void InventorySlotInit(string quickslot, bool isShop = false, bool isInventorySlot = false)
    {
        quickslotTxt.text = quickslot;
        this.isShop = isShop;
        this.isInventorySlot = isInventorySlot;
    }



    public void OnDrop(PointerEventData eventData)
    {
        GameObject target = eventData.pointerDrag;
        if (target.TryGetComponent<InventoryItem>(out var item))
        {
            InventoryItem currentInventoryItem = GetInventory();
            Item currentItem = GetInventoryItem();

            Item nextItem = item.GetItem();

            if (currentItem == null)
            {
                if (!isShop)
                {
                    if (item.IsShopItem())
                    {
                        BuyInventoryItem(item, nextItem);
                    }
                    else
                    {
                        item.rootParent = container;
                        item.ChangeIsInventoryItem(IsInventorySlot());
                    }
                }
                else
                {
                    bool isSellItem = item.IsShopItem();
                    if (!isSellItem)
                    {
                        int sell = item.GetTotalPrice();
                        CoinController.instance.AddCoin(sell);
                        Destroy(target);
                    }
                }
            }
            else
            {
                if (!isShop)
                {
                    if (item.IsShopItem())
                    {
                        BuyInventoryItem(item, nextItem);
                    }
                    else
                    {
                        if (currentItem.GetItemName() == nextItem.GetItemName())
                        {
                            int remain = currentInventoryItem.Add(item.GetCurrentQuantity());
                            if (remain == 0)
                            {
                                Destroy(target);
                            }
                            else
                            {
                                item.ChangeCurrentQuantity(remain);
                            }
                        }
                    }
                }
                else
                {
                    bool isSellItem = item.IsShopItem();
                    if (!isSellItem)
                    {
                        int sell = item.GetTotalPrice();
                        CoinController.instance.AddCoin(sell);
                        Destroy(target);
                    }
                }
            }
        }
    }
    public bool IsInventorySlot()
    {
        return isInventorySlot;
    }

    public void BuyInventoryItem(InventoryItem item, Item nextItem)
    {
        int buyPrice = item.GetSellPrice();
        bool isEnough = CoinController.instance.IsEnough(buyPrice);
        if (isEnough)
        {
            CoinController.instance.MinusCoin(buyPrice);
            int remain = InventoryController.instance.PickupItem(nextItem, 1);
            if (remain == 1)
            {
                CoinController.instance.AddCoin(buyPrice);
                LogController.instance.Log(LogMode.Inventory_Full);
            }
        }
        else
        {
            LogController.instance.Log(LogMode.Lack_Coin);
        }
    }

    public Item GetInventoryItem()
    {
        if (container.transform.childCount > 0 && container.transform.GetChild(0).gameObject.TryGetComponent<InventoryItem>(out var inventoryItem))
        {
            return inventoryItem.GetItem();
        }
        return null;
    }
    public InventoryItem GetInventory()
    {
        if (container.transform.childCount > 0 && container.transform.GetChild(0).gameObject.TryGetComponent<InventoryItem>(out var inventoryItem))
        {
            return inventoryItem;
        }
        return null;
    }

    public bool ExistItem()
    {
        if (container.transform.childCount > 0 && container.transform.GetChild(0).gameObject.GetComponent<InventoryItem>() != null)
        {
            return true;
        }
        return false;
    }
    public Transform GetItemContainer()
    {
        return container;
    }


}
