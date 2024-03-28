using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    public static InteractController instance;

    [SerializeField] private Transform interactTarget;

    Interactible targetItem = null;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }


    public Transform GetInteractTarget()
    {
        return interactTarget;
    }
    public void Interactible(Interactible target)
    {
        targetItem = target;
    }

    public void Interact()
    {

    }
}
