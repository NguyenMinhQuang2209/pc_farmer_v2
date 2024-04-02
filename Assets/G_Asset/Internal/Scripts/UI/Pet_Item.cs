using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pet_Item : MonoBehaviour
{
    [SerializeField] private Image pet_img;
    [SerializeField] private TextMeshProUGUI pet_name_txt;
    [SerializeField] private TextMeshProUGUI pet_level_txt;
    [SerializeField] private Slider pet_hungry_slider;
    [SerializeField] private TextMeshProUGUI pet_hungry_txt;
    [SerializeField] private Button detail_btn;
    [SerializeField] private Button feed_btn;
    private Pet currentPet;
    private void Start()
    {
        detail_btn.onClick.AddListener(() =>
        {
            ShowDetail();
        });
        feed_btn.onClick.AddListener(() =>
        {
            FeedPet();
        });
    }

    public void ShowDetail()
    {
        PetController.instance.ChoosePet(currentPet);
    }
    public void FeedPet()
    {

    }
    private void Update()
    {
        if (currentPet == null)
        {
            return;
        }
        pet_level_txt.text = currentPet.GetLevel();
        float currentFood = currentPet.GetCurrentFood();
        float maxFood = currentPet.GetMaxFood();
        pet_hungry_slider.maxValue = maxFood;
        pet_hungry_slider.value = currentFood;
        pet_hungry_txt.text = currentFood.ToString("0.0") + "/" + maxFood.ToString("0.0");
    }
    public Pet GetCurrentPet()
    {
        return currentPet;
    }

    public void PetInit(Pet newPet)
    {
        currentPet = newPet;
        pet_img.sprite = currentPet.GetSprite();
        pet_name_txt.text = currentPet.PetName();
        pet_hungry_slider.minValue = 0f;
    }
}
