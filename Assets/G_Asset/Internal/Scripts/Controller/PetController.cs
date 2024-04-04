using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour
{
    public static PetController instance;

    private readonly List<Pet_Item> pets = new();

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
            BuyPet(defaultPet[i], 0);
        }
    }

    public void BuyPet(Pet newPet, int price)
    {
        bool isEnough = CoinController.instance.IsEnough(price);

        if (isEnough)
        {
            CoinController.instance.MinusCoin(price);
            Transform player = PreferenceController.instance.GetPlayer();
            Pet pet = Instantiate(newPet, player.transform.position, Quaternion.identity);
            Pet_Item currentPetItem = Instantiate(pet_item, pet_container.transform);
            currentPetItem.PetInit(pet);
            pets.Add(currentPetItem);
        }
        else
        {
            LogController.instance.Log(LogMode.Lack_Coin);
        }
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
        FeedPet();
        ChoosePet(current_pet);
    }
    public void FeedPet()
    {
        Vector3 v = FoodController.instance.FeedPet();
        if (v != Vector3.zero)
        {
            current_pet.Feed(v.x, v.y, v.z);
        }
    }
    public void RecoverHealth()
    {
        int coin = current_pet.RecoverCoin();
        bool isEnough = CoinController.instance.IsEnough(coin);
        if (isEnough)
        {
            CoinController.instance.MinusCoin(coin);
            current_pet.RecoverAllHealth();
            ChoosePet(current_pet);
        }
        else
        {
            LogController.instance.Log(LogMode.Lack_Coin);
        }
    }
    public void ReleasePet()
    {
        Destroy(current_pet.gameObject);
        ChoosePet(null);
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
        current_pet.Upgrade();
        ChoosePet(current_pet);
    }

    public void ChangetPetMode(PetMode mode)
    {
        current_pet.ChangePetMode(mode);
        pet_status.PetStatusInit(current_pet);
    }

    public List<Pet_Item> GetPets()
    {
        return pets;
    }
    public void AddExeToPets(float v)
    {
        for (int i = 0; i < pets.Count; i++)
        {
            Pet_Item pet = pets[i];
            pet.AddExe(v);
        }
    }
}
