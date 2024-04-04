using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{
    public static LogController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void Log(string message)
    {
        Debug.Log(message);
    }
    public void Log(LogMode log)
    {
        switch (log)
        {
            case LogMode.Lack_Coin:
                Log("Thiếu vàng.");
                break;
            case LogMode.Inventory_Full:
                Log("Túi đã đầy");
                break;
            case LogMode.Lack_Item:
                Log("Thiếu item");
                break;

        }
    }
}
