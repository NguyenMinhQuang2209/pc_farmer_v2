using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pet_Status : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name_txt;
    [SerializeField] private Button follow_btn;
    [SerializeField] private Button protect_btn;
    [SerializeField] private Button stay_btn;
    [SerializeField] private Button patrol_btn;

    private void Start()
    {
        follow_btn.onClick.AddListener(() =>
        {
            ChangetPetMode(PetMode.Follow);
        });
        protect_btn.onClick.AddListener(() =>
        {
            ChangetPetMode(PetMode.Protect);
        });
        stay_btn.onClick.AddListener(() =>
        {
            ChangetPetMode(PetMode.StayInPosition);
        });
        patrol_btn.onClick.AddListener(() =>
        {
            ChangetPetMode(PetMode.Patrol);
        });
    }

    public void ChangetPetMode(PetMode mode)
    {
        PetController.instance.ChangetPetMode(mode);
    }

    public void PetStatusInit(Pet pet)
    {
        name_txt.text = "Trạng thái \n" + "(" + pet.GetPetMode() + ")";
    }
}
