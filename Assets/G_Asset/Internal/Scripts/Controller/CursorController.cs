using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;

    public static string Interactible_Str = "Interact";

    [HideInInspector] public string currentCursor = "";
    [SerializeField] private GameObject txt_container;
    [SerializeField] private TextMeshProUGUI txt;
    private List<GameObject> currents = new();
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
        txt_container.SetActive(false);
    }

    public void ChangeCursor(string newCursor, List<GameObject> newList)
    {
        UIController.instance.ChangeCursor.Invoke(null, null);
        if (newCursor != "")
        {
            BuildingController.instance.RemoveBuildingItem();
        }
        if (currentCursor.Contains(Interactible_Str))
        {
            InteractController.instance.CancelInteractItem();
        }

        if (currents != null)
        {
            for (int i = 0; i < currents.Count; i++)
            {
                currents[i].SetActive(false);
            }
        }
        if (currentCursor == newCursor)
        {
            currentCursor = "";
            currents = null;
            txt_container.SetActive(false);
            return;
        }
        currentCursor = newCursor;

        if (currentCursor != "Inventory")
        {
            txt.text = "Nhấn F để tắt túi đồ";
        }
        else
        {
            txt.text = "Nhấn Tab để tắt túi đồ";
        }
        txt_container.SetActive(currentCursor != "");

        currents = newList;
        if (currents != null)
        {
            for (int i = 0; i < currents.Count; i++)
            {
                currents[i].SetActive(true);
            }
        }
    }
    public string CurrentCursor()
    {
        return currentCursor;
    }
}
