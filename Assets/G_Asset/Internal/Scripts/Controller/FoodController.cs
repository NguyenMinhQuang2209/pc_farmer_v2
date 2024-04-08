using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public static FoodController instance;

    private PlayerHealth playerHealth;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void UseFoodItem(ItemName itemName, float v, float plusTime, float duration)
    {
        InventoryController.instance.RemoveItem(itemName.ToString(), 1);
        if (playerHealth == null)
        {
            Transform player = PreferenceController.instance.GetPlayer();
            if (player.TryGetComponent<PlayerHealth>(out playerHealth))
            {

            }
        }
        playerHealth.Feed(v, plusTime, duration);
    }

    public Vector3 FeedPet(ItemType foodType)
    {
        Vector3 value = Vector3.zero;
        int quantity = InventoryController.instance.GetRemainTypeQuantity(foodType);
        if (quantity > 0f)
        {
            string itemName = InventoryController.instance.GetInventoryItemNameByType(foodType);
            if (itemName != null)
            {
                Item item = PreferenceController.instance.GetItem(itemName);
                if (item != null && item.TryGetComponent<UseFood>(out var useFood))
                {
                    InventoryController.instance.RemoveItem(itemName, 1);
                    return useFood.GetValue();
                }
            }
        }
        LogController.instance.Log(LogMode.Lack_Item);
        return value;
    }
}
