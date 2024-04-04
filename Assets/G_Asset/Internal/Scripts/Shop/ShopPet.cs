using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPet : Interactible
{
    public override void Interact()
    {
        PetShopController.instance.InteractWithShop();
    }
}
