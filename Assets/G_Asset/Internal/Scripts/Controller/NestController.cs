using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NestController : MonoBehaviour
{
    public static NestController instance;

    [SerializeField] private Nest_Item item;
    [SerializeField] private GameObject nest_ui_container;
    [SerializeField] private Transform nest_tranform;
    private Nest previousNest = null;
    private Nest currentNest = null;

    private Dictionary<int, Nest_Item> storeItems = new();

    [Header("Buy Pet")]
    [SerializeField] private Image buyPetImg;
    [SerializeField] private TextMeshProUGUI growingTimeTxt;
    [SerializeField] private TextMeshProUGUI buyPriceTxt;
    [SerializeField] private Button buyFarmPetBtn;

    [Header("Collect")]
    [SerializeField] private Image collectImg;
    [SerializeField] private TextMeshProUGUI quantityTxt;
    [SerializeField] private Button collectBtn;
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
        foreach (Transform child in nest_tranform)
        {
            Destroy(child.gameObject);
        }
        nest_ui_container.SetActive(true);
        collectBtn.onClick.AddListener(() =>
        {
            CollectItem();
        });
        buyFarmPetBtn.onClick.AddListener(() =>
        {
            BuyFarmPet();
        });
        nest_ui_container.SetActive(false);
    }

    public void CollectItem()
    {
        if (currentNest != null)
        {
            currentNest.CollectQuantity();
        }
    }
    private void Update()
    {
        if (currentNest != null)
        {
            quantityTxt.text = "Số lượng: " + currentNest.GetTotalQuantity();
        }
    }
    public void BuyFarmPet()
    {
        bool isEnough = CoinController.instance.IsEnoughAndMinus(currentNest.GetBuyPet().price);
        if (isEnough)
        {
            currentNest.AddPetFarm();
            LoadNestItem(false);
        }
        else
        {
            LogController.instance.Log(LogMode.Lack_Coin);
        }
    }
    public void ChangeNest(Nest newNest)
    {
        bool isEnd = false;
        if (currentNest != null)
        {
            previousNest = currentNest;
        }
        currentNest = newNest;
        if (currentNest == null)
        {
            isEnd = true;
        }

        if (isEnd)
        {
            currentNest = null;
            CursorController.instance.ChangeCursor("", null);
            return;
        }
        CursorController.instance.ChangeCursor("Nest_", new() { nest_ui_container });

        LoadNestItem();
    }
    public void LoadNestItem(bool check = true)
    {
        if (check)
        {
            if (currentNest == previousNest)
            {
                return;
            }
        }
        BuyFarmPetItem buyPet = currentNest.GetBuyPet();
        buyPetImg.sprite = buyPet.img;
        growingTimeTxt.text = "Thời gian lớn: " + buyPet.GetTotalGrowingTime() + "s";
        buyPriceTxt.text = "Mua " + buyPet.price.ToString();

        collectImg.sprite = currentNest.collectSprite;
        quantityTxt.text = "Số lượng: " + currentNest.GetTotalQuantity();

        List<FarmPet> items = currentNest.GetNestList();
        int childNumber = nest_tranform.childCount;
        int maxIndex = childNumber > items.Count ? childNumber : items.Count;
        for (int i = 0; i < maxIndex; i++)
        {
            if (items.Count > i)
            {
                FarmPet currentFarmPet = items[i];
                Nest_Item currentItem;
                if (storeItems.ContainsKey(i))
                {
                    currentItem = storeItems[i];
                    nest_tranform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    currentItem = Instantiate(item, nest_tranform.transform, false);
                }
                storeItems[i] = currentItem;
                currentItem.NestItemInit(currentFarmPet);
            }
            else
            {
                nest_tranform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    public void SellItem(int v, FarmPet pet)
    {
        CoinController.instance.AddCoin(v);
        currentNest.LoadNest(pet);
        LoadNestItem(false);
    }
}
