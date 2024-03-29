using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [HideInInspector] public Transform rootParent;
    private Item item = null;

    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI quantityTxt;

    private void Start()
    {
        rootParent = transform.parent;
    }

    public void InventoryItemInit(Item item)
    {
        this.item = item;
        img.sprite = item.sprite;
        quantityTxt.text = item.UseStack() ? item.GetCurrentQuantity().ToString() : "";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rootParent = transform.parent;
        Transform tempParent = InventoryController.instance.drag_parent;
        transform.SetParent(tempParent);
        transform.SetAsLastSibling();
        quantityTxt.text = "";
        img.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        quantityTxt.text = item.UseStack() ? item.GetCurrentQuantity().ToString() : "";
        img.raycastTarget = true;
        transform.SetParent(rootParent);
    }

    public string GetItemName()
    {
        if (item != null)
        {
            return item.itemName.ToString();
        }
        return null;
    }
}
