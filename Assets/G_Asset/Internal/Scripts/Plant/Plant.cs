using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Interactible
{
    public override void Interact()
    {
        LogController.instance.Log("Plant Tree");
    }
}
