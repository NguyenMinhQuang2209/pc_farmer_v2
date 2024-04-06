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
    private Item item = null;
    private int currentQuantity = 1;

    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI quantityTxt;
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private GameObject buy_ui;
    [SerializeField] private int sellPrice = 0;
    [SerializeField] private bool isShopItem = false;

    private bool isInventory = false;


    bool areDrag = false;

    private void Start()
    {
        rootParent = transform.parent;
    }

    private void Update()
    {
        if (item != null && !areDrag)
        {
            quantityTxt.text = isShopItem ? "" : item.UseStack() ? currentQuantity.ToString() : "";
        }
    }

    public void InventoryItemInit(Item item, int quantity, bool isShopItem, bool isInventory = false)
    {
        this.isInventory = isInventory;
        this.item = item;
        img.sprite = item.sprite;
        this.isShopItem = isShopItem;
        buy_ui.SetActive(isShopItem);
        currentQuantity = quantity;
        if (this.isShopItem)
        {
            sellPrice = this.item.GetBuyPricePerUnit();
            priceTxt.text = sellPrice.ToString();
        }
    }
    public int GetTotalPrice()
    {
        return currentQuantity * item.GetSellPricePerUnit();
    }
    public int GetCurrentQuantity()
    {
        return currentQuantity;
    }
    public void ChangeCurrentQuantity(int v)
    {
        currentQuantity = Mathf.Min(v, item.GetMaxQuantity());
    }
    public void ChangeIsInventoryItem(bool v)
    {
        isInventory = v;
    }
    public int Add(int v)
    {
        if (item != null)
        {
            int max = item.GetMaxQuantity();
            int next = currentQuantity + v;
            if (next > max)
            {
                currentQuantity = max;
                return next - max;
            }
            else
            {
                currentQuantity = next;
                return 0;
            }
        }
        return v;
    }

    public int Minus(int v)
    {
        if (item != null)
        {
            int next = currentQuantity - v;
            if (next < 0)
            {
                currentQuantity = 0;
                return next * -1;
            }
            else
            {
                currentQuantity = next;
                return 0;
            }
        }
        return v;
    }
    public void CheckQuantity()
    {
        if (currentQuantity == 0)
        {
            Destroy(gameObject);
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

    public Item GetItem()
    {
        return item;
    }
    public void UseItem()
    {
        if (item != null && isInventory && item.TryGetComponent<Use_Item>(out var useItem))
        {
            useItem.UseItem();
        }
    }

    public bool CanAdd(int v)
    {
        int max = item.GetMaxQuantity();
        return currentQuantity + v <= max;
    }
}
