using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractTarget : MonoBehaviour
{
    private Interactible interactTarget;
    [SerializeField] private TextMeshProUGUI interactTxt;
    private void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    private void Update()
    {
        if (interactTarget != null)
        {
            interactTxt.text = interactTarget.promptMessage;
        }
        else
        {
            interactTxt.text = "";
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactTarget = collision.gameObject.GetComponent<Interactible>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        interactTarget = null;
    }

    public void Interact()
    {
        if (interactTarget != null)
        {
            interactTarget.BaseInteract();
        }
    }
}
