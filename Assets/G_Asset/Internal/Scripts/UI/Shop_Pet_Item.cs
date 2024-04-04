using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Pet_Item : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private Button buyBtn;
    [SerializeField] private Button showDetailBtn;
    private PetShopItem petShopItem = null;
    private void Start()
    {
        buyBtn.onClick.AddListener(() =>
        {
            BuyItem();
        });
        showDetailBtn.onClick.AddListener(() =>
        {
            ShowDetail();
        });
    }
    public void ShopPetItemInit(PetShopItem item)
    {
        petShopItem = item;
        img.sprite = petShopItem.sprite;
        priceTxt.text = petShopItem.price.ToString();
        nameTxt.text = petShopItem.pet.PetName();
    }

    public void BuyItem()
    {
        PetController.instance.BuyPet(petShopItem.pet, petShopItem.price);
    }
    public void ShowDetail()
    {
        PetShopController.instance.ChangePetDetail(petShopItem);
    }
}
