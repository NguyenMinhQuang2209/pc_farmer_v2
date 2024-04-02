using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public static CoinController instance;

    [SerializeField] private int currentCoin = 0;
    [SerializeField] private TextMeshProUGUI coinTxt;
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
        coinTxt.text = currentCoin.ToString();
    }
    public bool IsEnough(int v)
    {
        return currentCoin >= v;
    }
    public void MinusCoin(int v)
    {
        currentCoin = Mathf.Max(currentCoin - v, 0);
        coinTxt.text = currentCoin.ToString();
    }
    public void AddCoin(int v)
    {
        currentCoin += v;
        coinTxt.text = currentCoin.ToString();
    }
}
