using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactible
{
    [SerializeField] private bool useThisSprite = false;
    [SerializeField] private Item item;
    [SerializeField] private int quantity = 1;
    private void Start()
    {
        if (useThisSprite)
        {
            item.sprite = GetComponent<SpriteRenderer>().sprite;
        }
    }

    public override void Interact()
    {
        int remain = InventoryController.instance.PickupItem(item, quantity);
        if (remain == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            quantity = remain;
        }
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
}
