using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUIWorldController : MonoBehaviour
{
    public static ShowUIWorldController instance;
    public ShowUIWorld worldItem;

    private List<ShowUIWorld> poolingItems = new();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void ShowWorldTextItem(Vector3 pos, string txt, Color color, float timer = -1f)
    {
        ShowUIWorld item;
        if (poolingItems.Count > 0)
        {
            item = poolingItems[0];
            poolingItems.RemoveAt(0);
            item.gameObject.SetActive(true);
        }
        else
        {
            item = Instantiate(worldItem, pos, Quaternion.identity);
        }
        item.ShowUIWorldInit(pos, txt, color, timer);
    }
    public void AddPoolingItem(ShowUIWorld item)
    {
        poolingItems.Add(item);
    }
    public void ShowWorldDamageTextItem(Vector3 pos, string txt, float timer = -1f)
    {
        ShowWorldTextItem(pos, txt, Color.red, timer);
    }
    public void ShowWorldHealTextItem(Vector3 pos, string txt, float timer = -1f)
    {
        ShowWorldTextItem(pos, txt, Color.green, timer);
    }
}
