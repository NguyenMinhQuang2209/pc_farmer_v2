using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public GameObject pet;
    public GameObject inventory;
    public GameObject update_ui;

    public RectTransform detail_ui;
    public TextMeshProUGUI detail_ui_txt;

    private InventoryItem currentItem = null;

    public EventHandler ChangeCursor;
    private RectTransform rect;
    [SerializeField] private Vector2 offset = new();
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
        pet.SetActive(true);
        pet.SetActive(false);
        ChangeCursor += HandleChangeCursor;
        ChangeShowDetail(null);
    }
    public void ChangeShowDetail(InventoryItem nextItem)
    {
        currentItem = nextItem;
        if (currentItem != null)
        {
            detail_ui_txt.text = currentItem.GetShowName();
            if (currentItem.TryGetComponent<RectTransform>(out rect))
            {
                detail_ui.position = new(rect.position.x + offset.x, rect.position.y + offset.y, rect.position.z);
            }
        }
        detail_ui.gameObject.SetActive(currentItem != null);
    }
    private void HandleChangeCursor(object sender, EventArgs e)
    {
        PetController.instance.ChoosePet(null, -1);
        ChangeShowDetail(null);
        inventory.SetActive(true);
        pet.SetActive(false);
        update_ui.SetActive(false);
    }

    public void OpenInventory()
    {
        inventory.SetActive(true);
        pet.SetActive(false);
        update_ui.SetActive(false);
    }
    public void OpenPet()
    {
        inventory.SetActive(false);
        pet.SetActive(true);
        update_ui.SetActive(false);
    }

    public void OpenUpdate()
    {
        inventory.SetActive(false);
        pet.SetActive(false);
        update_ui.SetActive(true);
    }
}
