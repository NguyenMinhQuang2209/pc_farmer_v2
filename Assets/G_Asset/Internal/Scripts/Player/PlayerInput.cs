using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions action;
    public PlayerInputActions.OnFootActions onFoot;
    private PlayerMovement playerMovement;
    private void Awake()
    {
        action = new PlayerInputActions();
        onFoot = action.onFoot;
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (onFoot.UI.triggered)
        {
            InventoryController.instance.InteractWithInventory();
        }
    }
    private void FixedUpdate()
    {
        playerMovement.Movement(onFoot.Movement.ReadValue<Vector2>().normalized);
    }
    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
