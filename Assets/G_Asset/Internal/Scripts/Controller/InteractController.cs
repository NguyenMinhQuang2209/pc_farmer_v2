using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractController : MonoBehaviour
{
    public static InteractController instance;

    private Interactible current_item = null;

    private Interactible destroy_item = null;

    [SerializeField] private GameObject destroy_ui_gameobject;
    [SerializeField] private TextMeshProUGUI destroy_ui_txt;
    [SerializeField] private Button destroy_btn;
    [SerializeField] private Button cancel_btn;
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
        destroy_ui_gameobject.SetActive(true);
        destroy_btn.onClick.AddListener(() =>
        {
            InteractDestroy(true);
        });
        cancel_btn.onClick.AddListener(() =>
        {
            InteractDestroy(false);
        });
        destroy_ui_gameobject.SetActive(false);
    }
    public void InteractItem(Interactible item)
    {
        if (current_item != null)
        {
            current_item.CancelInteract();
        }
        current_item = item;
    }
    public void CancelInteractItem()
    {
        if (current_item != null)
        {
            current_item.CancelInteract();
        }
        current_item = null;
    }

    public void DestroyItem(Interactible item)
    {
        destroy_item = item;
        if (destroy_item != null)
        {
            destroy_ui_txt.text = "Bạn có muốn xóa " + destroy_item.interact_name + " không ?";
        }
        destroy_ui_gameobject.SetActive(destroy_item != null);
    }
    public void InteractDestroy(bool v)
    {
        if (v)
        {
            Destroy(destroy_item.gameObject);
        }
        DestroyItem(null);
    }
}
