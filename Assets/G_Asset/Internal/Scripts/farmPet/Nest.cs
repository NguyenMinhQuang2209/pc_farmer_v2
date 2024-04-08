using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : Interactible
{
    private List<FarmPet> nestItems = new();
    [SerializeField] private Transform spawnPosition;

    public Sprite collectSprite;
    [SerializeField] private Item collectItem;
    [SerializeField] private BuyFarmPetItem buyPet = new();

    int totalQuantity = 0;

    public override void Interact()
    {
        NestController.instance.ChangeNest(this);
    }
    public List<FarmPet> GetNestList()
    {
        return nestItems;
    }
    public BuyFarmPetItem GetBuyPet()
    {
        return buyPet;
    }
    public int GetTotalQuantity()
    {
        return totalQuantity;
    }
    public void AddPetFarm()
    {
        FarmPet farmPet = Instantiate(buyPet.farmPet, spawnPosition.position, Quaternion.identity);
        farmPet.FarmPetInit(transform);
        nestItems.Add(farmPet);
    }
    public void LoadNest(FarmPet pet)
    {
        for (int i = 0; i < nestItems.Count; i++)
        {
            if (nestItems[i] == pet)
            {
                nestItems.RemoveAt(i);
                i--;
            }
        }
        Destroy(pet.gameObject);
    }
    public void ProduceProduct(int quantity)
    {
        totalQuantity += quantity;
    }
    public void CollectQuantity()
    {
        if (totalQuantity > 0)
        {
            int remain = InventoryController.instance.PickupItem(collectItem, totalQuantity);
            totalQuantity = remain;
        }
    }
}
[System.Serializable]
public class BuyFarmPetItem
{
    public Sprite img;
    public int price = 0;
    public FarmPet farmPet;
    public float GetTotalGrowingTime()
    {
        return farmPet.GetTotalTime();
    }
}
