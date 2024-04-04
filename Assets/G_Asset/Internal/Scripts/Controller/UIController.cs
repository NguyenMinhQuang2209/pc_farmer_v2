using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public GameObject pet;
    public GameObject inventory;
    public GameObject update_ui;

    public EventHandler ChangeCursor;
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
    }

    private void HandleChangeCursor(object sender, EventArgs e)
    {
        PetController.instance.ChoosePet(null, -1);
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
