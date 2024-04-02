using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour
{
    public static PetController instance;

    private List<Pet_Item> pets = new();

    [SerializeField] private Pet_Item pet_item;
    [SerializeField] private Transform pet_container;
    [SerializeField] private Pet_Status pet_status;
    [SerializeField] private Pet_Detail pet_detail;

    [SerializeField] private List<Pet> defaultPet = new();

    private Pet current_pet = null;
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
        pet_status.gameObject.SetActive(true);
        pet_detail.gameObject.SetActive(true);
        foreach (Transform child in pet_container)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < defaultPet.Count; i++)
        {
            BuyPet(defaultPet[i]);
        }
    }

    public void BuyPet(Pet newPet)
    {
        Transform player = PreferenceController.instance.GetPlayer();
        Pet pet = Instantiate(newPet, player.transform.position, Quaternion.identity);
        Pet_Item currentPetItem = Instantiate(pet_item, pet_container.transform);
        currentPetItem.PetInit(pet);
        pets.Add(currentPetItem);
    }
    public void CheckPet()
    {
        List<Pet_Item> tempStoreItem = new();
        for (int i = 0; i < pets.Count; i++)
        {
            if (pets[i].GetCurrentPet() == null)
            {
                Pet_Item currentItem = pets[i];
                tempStoreItem.Add(currentItem);
                pets.RemoveAt(i);
                i--;
            }
        }
        if (tempStoreItem.Count > 0)
        {
            for (int i = 0; i < tempStoreItem.Count; i++)
            {
                Destroy(tempStoreItem[i].gameObject);
            }
        }
    }

    public void ChoosePet(Pet newPet)
    {
        current_pet = newPet;
        CheckCurrentPet();
        if (current_pet != null)
        {
            pet_detail.ChangePetDetail(current_pet);
            pet_status.PetStatusInit(current_pet);
        }
    }

    public void RecoverFood()
    {
    }
    public void RecoverHealth()
    {

    }
    public void ReleasePet()
    {
        Destroy(current_pet.gameObject);
        current_pet = null;
        CheckCurrentPet();
        CheckPet();
    }
    public void CheckCurrentPet()
    {
        pet_detail.gameObject.SetActive(current_pet != null);
        pet_status.gameObject.SetActive(current_pet != null);
    }
    public void UpgradePet()
    {

    }

    public void ChangetPetMode(PetMode mode)
    {
        current_pet.ChangePetMode(mode);
        pet_status.PetStatusInit(current_pet);
    }
}
