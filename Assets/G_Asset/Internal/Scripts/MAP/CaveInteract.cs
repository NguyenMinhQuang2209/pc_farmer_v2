using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveInteract : Interactible
{
    public int currentIndex = 0;
    public override void Interact()
    {
        MapController.instance.InteractWithMap(currentIndex);
    }
}
