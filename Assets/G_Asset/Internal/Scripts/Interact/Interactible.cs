using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    public string promptMessage = "F để tương tác";
    [SerializeField] protected bool useTrigger = false;
    [SerializeField] protected bool canDestroy = false;

    public string interact_name = "";
    //public bool useAnimator = false;
    public void BaseInteract()
    {
        Interact();
    }

    public virtual void Interact()
    {

    }
    public virtual void CancelInteract()
    {

    }
    public virtual void InteractInit()
    {

    }
    public virtual void InteractDestroy()
    {
        if (canDestroy)
        {
            InteractController.instance.DestroyItem(this);
        }
    }
}
