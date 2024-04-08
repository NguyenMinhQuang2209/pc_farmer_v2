using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Nest_Item : MonoBehaviour
{
    public Image img;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI timeTxt;
    public Slider foodSlider;
    public Button feedBtn;
    public Button sellBtn;
    public TextMeshProUGUI sellTxt;
    private FarmPet currentFarmPet = null;
    private void Start()
    {
        feedBtn.onClick.AddListener(() =>
        {
            FeedFarmPet();
        });
        sellBtn.onClick.AddListener(() =>
        {
            SellFarmPet();
        });
        foodSlider.minValue = 0f;
    }
    private void Update()
    {
        if (currentFarmPet != null)
        {
            nameTxt.text = currentFarmPet.petName;
            timeTxt.text = currentFarmPet.GetGrowingRemainTime();
            foodSlider.value = currentFarmPet.GetCurrentFood();
            sellTxt.text = "Bán " + currentFarmPet.GetCurrentPrice();
        }
    }

    public void NestItemInit(FarmPet newFarmpet)
    {
        currentFarmPet = newFarmpet;
        img.sprite = currentFarmPet.img;
        foodSlider.maxValue = currentFarmPet.GetMaxFood();
        sellTxt.text = "Bán " + currentFarmPet.GetCurrentPrice();
    }
    public FarmPet GetFarmPet()
    {
        return currentFarmPet;
    }

    public void FeedFarmPet()
    {
        FeedPet();
    }
    public void FeedPet()
    {
        Vector3 v = FoodController.instance.FeedPet(ItemType.Food);
        if (v != Vector3.zero)
        {
            currentFarmPet.Feed(v.x);
        }
    }
    public void SellFarmPet()
    {
        NestController.instance.SellItem(currentFarmPet.GetCurrentPrice(), currentFarmPet);
    }
}
