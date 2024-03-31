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
        InventoryController.instance.InteractWithChest(this, currentSlot);
    }
    public override void CancelInteract()
    {
        animator.SetBool("Open", false);
        items = InventoryController.instance.GetChestListItem(currentSlot);
    }
}
