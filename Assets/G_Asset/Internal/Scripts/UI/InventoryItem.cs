using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [HideInInspector] public Transform rootParent;
    private ItemInit item = null;
    private Item rootItem = null;

    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI quantityTxt;
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private GameObject buy_ui;
    [SerializeField] private int sellPrice = 0;
    [SerializeField] private bool isShopItem = false;


    bool areDrag = false;

    private void Start()
    {
        rootParent = transform.parent;
    }

    private void Update()
    {
        if (item != null && !areDrag)
        {
            quantityTxt.text = isShopItem ? "" : item.UseStack() ? item.GetCurrentQuantity().ToString() : "";
        }
    }

    public void InventoryItemInit(Item item, int quantity, bool isShopItem)
    {
        rootItem = item;
        this.item = new(item.sprite, item.itemName, item.currentQuantity, item.maxQuantity, item.price, item.buyRate);
        img.sprite = item.sprite;
        this.isShopItem = isShopItem;
        buy_ui.SetActive(isShopItem);
        this.item.ChangeItemQuantity(quantity);
        if (this.isShopItem)
        {
            sellPrice = this.item.GetBuyPricePerUnit();
            priceTxt.text = sellPrice.ToString();
        }
    }

    public void InventoryItemInit(ItemInit item, int quantity, bool isShopItem)
    {
        this.item = item.Clone();
        this.item.ChangeItemQuantity(quantity);
        img.sprite = item.sprite;
        this.isShopItem = isShopItem;
        buy_ui.SetActive(isShopItem);
        this.item.ChangeItemQuantity(quantity);
        if (this.isShopItem)
        {
            sellPrice = this.item.GetBuyPricePerUnit();
            priceTxt.text = sellPrice.ToString();
        }
    }
    public int GetSellPrice()
    {
        return sellPrice;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rootParent = transform.parent;
        Transform tempParent = InventoryController.instance.drag_parent;
        transform.SetParent(tempParent);
        transform.SetAsLastSibling();
        quantityTxt.text = "";
        img.raycastTarget = false;
        areDrag = true;
        buy_ui.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        img.raycastTarget = true;
        transform.SetParent(rootParent);
        areDrag = false;
        if (isShopItem)
        {
            buy_ui.SetActive(true);
        }
    }
    public bool IsShopItem()
    {
        return isShopItem;
    }

    public string GetItemName()
    {
        if (item != null)
        {
            return item.itemName.ToString();
        }
        return null;
    }
    public ItemInit GetItemInit()
    {
        return item;
    }

    public Item GetItem()
    {
        return rootItem;
    }

    public void UseItem()
    {
        if (rootItem != null)
        {
            rootItem.UseItem();
        }
    }
}
