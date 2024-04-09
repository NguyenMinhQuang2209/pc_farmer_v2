using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveInteract : Interactible
{
    public override void Interact()
    {
        MapController.instance.InteractWithMap();
    }
}
