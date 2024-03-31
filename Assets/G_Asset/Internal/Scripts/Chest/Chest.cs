using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactible
{
    private Animator animator;
    [SerializeField]
    private int currentSlot = 1;
    private List<InventoryItem> items = new();
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public override void Interact()
    {
        animator.SetBool("Open", true);
        InteractController.instance.InteractItem(this);
        InventoryController.instance.InteractWithChest(this);
    }
    public override void CancelInteract()
    {
        animator.SetBool("Open", false);
    }

    public void SetListItems(List<InventoryItem> list)
    {
        items = list;
    }
    public int GetCurrentSlot()
    {
        return currentSlot;
    }
    public List<InventoryItem> GetListItems()
    {
        return items;
    }
}
