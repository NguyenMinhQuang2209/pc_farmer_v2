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
    }
    private void Update()
    {
        if (currentFarmPet != null)
        {
            nameTxt.text = currentFarmPet.petName;
            timeTxt.text = currentFarmPet.GetGrowingRemainTime();
        }
    }

    public void NestItemInit(FarmPet newFarmpet)
    {
        currentFarmPet = newFarmpet;
        img.sprite = currentFarmPet.img;

    }

    public void FeedFarmPet()
    {

    }
    public void SellFarmPet()
    {

    }
}
