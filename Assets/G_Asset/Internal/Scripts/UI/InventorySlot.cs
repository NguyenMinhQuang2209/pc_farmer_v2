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

    public void InventorySlotInit(string quickslot, bool isShop = false)
    {
        quickslotTxt.text = quickslot;
        this.isShop = isShop;
    }



    public void OnDrop(PointerEventData eventData)
    {
        GameObject target = eventData.pointerDrag;
        if (target.TryGetComponent<InventoryItem>(out var item))
        {
            ItemInit currentItem = GetInventoryItemInit();
            ItemInit nextItem = item.GetItemInit();
            Item nextItemRoot = item.GetItem();
            if (currentItem == null)
            {
                if (!isShop)
                {
                    if (item.IsShopItem())
                    {
                        int buyPrice = item.GetSellPrice();
                        bool isEnough = CoinController.instance.IsEnough(buyPrice);
                        if (isEnough)
                        {
                            CoinController.instance.MinusCoin(buyPrice);
                            InventoryController.instance.PickupItem(nextItemRoot, 1);
                        }
                        else
                        {
                            LogController.instance.Log(LogMode.Lack_Coin);
                        }
                    }
                    else
                    {
                        item.rootParent = container;
                    }
                }
                else
                {
                    bool isSellItem = item.IsShopItem();
                    if (!isSellItem)
                    {
                        int sell = nextItem.GetTotalPrice();
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
                        if (currentItem.CanAdd())
                        {
                            int buyPrice = item.GetSellPrice();
                            bool isEnough = CoinController.instance.IsEnough(buyPrice);
                            if (isEnough)
                            {
                                CoinController.instance.MinusCoin(buyPrice);
                                currentItem.Add(1);
                            }
                            else
                            {
                                LogController.instance.Log(LogMode.Lack_Coin);
                            }
                        }
                    }
                    else
                    {
                        int remain = currentItem.Add(nextItem.GetCurrentQuantity());
                        if (remain == 0)
                        {
                            Destroy(target);
                        }
                        else
                        {
                            nextItem.ChangeItemQuantity(remain);
                        }
                    }
                }
                else
                {
                    bool isSellItem = item.IsShopItem();
                    if (!isSellItem)
                    {
                        int sell = nextItem.GetTotalPrice();
                        CoinController.instance.AddCoin(sell);
                        Destroy(target);
                    }
                }
            }
        }
    }
    public ItemInit GetInventoryItemInit()
    {
        if (container.transform.childCount > 0 && container.transform.GetChild(0).gameObject.TryGetComponent<InventoryItem>(out var inventoryItem))
        {
            return inventoryItem.GetItemInit();
        }
        return null;
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
