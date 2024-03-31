using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    public static InteractController instance;

    private Interactible current_item = null;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void InteractItem(Interactible item)
    {
        if (current_item != null)
        {
            current_item.CancelInteract();
        }
        current_item = item;
    }
    public void CancelInteractItem()
    {
        if (current_item != null)
        {
            current_item.CancelInteract();
        }
        current_item = null;
    }
}
