using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    public string promptMessage = "";
    [SerializeField] protected bool useTrigger = false;
    public bool useAnimator = false;
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
}
