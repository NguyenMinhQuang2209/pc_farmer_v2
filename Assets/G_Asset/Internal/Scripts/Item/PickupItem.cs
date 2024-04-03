using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PickupItem : Interactible
{
    [SerializeField] private List<PickupItemDetail> items = new();
    private void Start()
    {
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
        }
    }

    public override void Interact()
    {
        Pickup();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (useTrigger)
        {
            if (collision.gameObject.GetComponent<PlayerMovement>() != null)
            {
                BaseInteract();
            }
        }
    }
    public void Pickup()
    {
        bool isPickupAll = true;
        for (int i = 0; i < items.Count; i++)
        {
            PickupItemDetail itemDetail = items[i];
            int remain = InventoryController.instance.PickupItem(itemDetail.item, itemDetail.quantity);
            if (remain > 0)
            {
                itemDetail.quantity = remain;
                isPickupAll = false;
            }
            else
            {
                items.RemoveAt(i);
                i--;
            }
        }
        if (isPickupAll)
        {
            Destroy(gameObject);
        }
    }
}
[System.Serializable]
public class PickupItemDetail
{
    public Item item;
    public int quantity;

    [Header("Use random quantity")]
    public bool useRandom = false;
    public Vector2Int quantityRange = new();
}