using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{

    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI quickslotTxt;

    public void InventorySlotInit(string quickslot)
    {
        quickslotTxt.text = quickslot;
    }



    public void OnDrop(PointerEventData eventData)
    {
        GameObject target = eventData.pointerDrag;
        if (target.TryGetComponent<InventoryItem>(out var item))
        {
            Item currentItem = GetInventoryItem();
            Item nextItem = item.GetItem();
            if (currentItem == null)
            {
                item.rootParent = container;
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
    }
    public Item GetInventoryItem()
    {
        if (container.transform.childCount > 0 && container.transform.GetChild(0).gameObject.TryGetComponent<InventoryItem>(out var inventoryItem))
        {
            return inventoryItem.GetItem();
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
