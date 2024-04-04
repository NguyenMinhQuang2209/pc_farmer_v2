using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceController : MonoBehaviour
{
    public static PreferenceController instance;

    public static string PLAYER_TAG = "Player";

    [HideInInspector] public Transform player;
    [SerializeField] private List<PreferenceItem> items = new();
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
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
    }
    public Transform GetPlayer()
    {
        return GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
    }
    public Item GetItem(string itemName)
    {
        for (int i = 0; i < items.Count; i++)
        {
            PreferenceItem item = items[i];
            if (item.item.GetItemName() == itemName)
            {
                return item.item;
            }
        }
        return null;
    }

    public Item GetItem(ItemName itemName)
    {
        for (int i = 0; i < items.Count; i++)
        {
            PreferenceItem item = items[i];
            if (item.item.GetName() == itemName)
            {
                return item.item;
            }
        }
        return null;
    }
}
[System.Serializable]
public class PreferenceItem
{
    public Item item;
    public ItemName itemName;
}
