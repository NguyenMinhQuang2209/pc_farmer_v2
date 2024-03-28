using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Transform interactTarget;
    private PlayerInput playerInput;
    private void Start()
    {
        interactTarget = InteractController.instance.GetInteractTarget();
        playerInput = GetComponent<PlayerInput>();
    }
    private void Update()
    {
        Vector2 mousePostion = Input.mousePosition;

        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePostion);

        worldPosition.x = Mathf.Round(worldPosition.x / 0.16f) * 0.16f;
        worldPosition.y = Mathf.Round(worldPosition.y / 0.16f) * 0.16f;

        interactTarget.position = worldPosition;

        if (playerInput.onFoot.Build.triggered)
        {
            InteractController.instance.Interact();
        }

    }
}
