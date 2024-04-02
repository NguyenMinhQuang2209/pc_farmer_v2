using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pet_Detail : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name_txt;
    [SerializeField] private Slider exe_slider;
    [SerializeField] private TextMeshProUGUI exe_txt;
    [SerializeField] private TextMeshProUGUI detail_txt;
    [SerializeField] private Button feed_btn;
    [SerializeField] private Button release_btn;
    [SerializeField] private Button recover_btn;
    [SerializeField] private TextMeshProUGUI recover_txt;
    [SerializeField] private Button upgrade_btn;
    [SerializeField] private TextMeshProUGUI upgrade_txt;
    private void Start()
    {
        feed_btn.onClick.AddListener(() =>
        {
            RecoverFood();
        });

        recover_btn.onClick.AddListener(() =>
        {
            RecoverFood();
        });

        release_btn.onClick.AddListener(() =>
        {
            ReleasePet();
        });

        upgrade_btn.onClick.AddListener(() =>
        {
            UpgradePet();
        });
    }

    public void ChangePetDetail(Pet newPet)
    {
        name_txt.text = newPet.PetName();
        exe_slider.maxValue = newPet.GetNextExe();
        exe_slider.minValue = 0f;
        exe_slider.value = newPet.GetCurrentExe();
        exe_txt.text = newPet.GetCurrentExe().ToString("0.0") + "/" + newPet.GetNextExe().ToString("0.0");

        detail_txt.text = newPet.GetPetDetail();

        if (!newPet.IsMaxLevel())
        {
            upgrade_txt.text = "Nâng cấp " + newPet.GetNextPrice().ToString();
        }
        recover_txt.text = "Hồi phục " + newPet.RecoverCoin();

    }
    public void RecoverFood()
    {
        PetController.instance.RecoverFood();
    }
    public void RecoverHealth()
    {
        PetController.instance.RecoverHealth();
    }
    public void ReleasePet()
    {
        PetController.instance.ReleasePet();
    }
    public void UpgradePet()
    {
        PetController.instance.UpgradePet();
    }
}
