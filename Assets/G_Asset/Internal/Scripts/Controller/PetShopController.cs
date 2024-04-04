using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetShopController : MonoBehaviour
{
    public static PetShopController instance;


    [SerializeField] private GameObject petshop_ui_container;
    [SerializeField] private Transform petShop_ui;
    [SerializeField] private Shop_Pet_Item item;

    [SerializeField] private GameObject detail_ui;
    [SerializeField] private TextMeshProUGUI detail_name_txt;
    [SerializeField] private TextMeshProUGUI detail_detail_txt;
    [SerializeField] private TextMeshProUGUI detail_price_txt;
    [SerializeField] private Button buyBtn;

    [SerializeField] private List<PetShopItem> items = new();

    private PetShopItem currentItem = null;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        petshop_ui_container.SetActive(true);
        foreach (Transform child in petShop_ui.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < items.Count; i++)
        {
            PetShopItem shopItem = items[i];
            Shop_Pet_Item tempItem = Instantiate(item, petShop_ui.transform, false);
            tempItem.ShopPetItemInit(shopItem);
        }
        buyBtn.onClick.AddListener(() =>
        {
            BuyItem();
        });
        ChangePetDetail(null);
        petshop_ui_container.SetActive(false);
    }
    public void InteractWithShop()
    {
        ChangePetDetail(null);
        CursorController.instance.ChangeCursor("PetShop", new() { petshop_ui_container });
    }
    public void BuyItem()
    {
        if (currentItem != null)
        {
            PetController.instance.BuyPet(currentItem.pet, currentItem.price);
        }
    }
    public void ChangePetDetail(PetShopItem newItem)
    {
        currentItem = newItem;
        if (currentItem != null)
        {
            detail_name_txt.text = currentItem.pet.PetName();
            detail_detail_txt.text = currentItem.pet.GetPetDetail(false);
            detail_price_txt.text = currentItem.price.ToString();
        }
        detail_ui.SetActive(currentItem != null);
    }

}
[System.Serializable]
public class PetShopItem
{
    public int price = 1;
    public Sprite sprite;
    public Pet pet;
}
