using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions action;
    public PlayerInputActions.OnFootActions onFoot;
    private PlayerMovement playerMovement;

    private Animator toolAnimator;

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

        if (onFoot.Hit.IsPressed())
        {
            string cursor = CursorController.instance.CurrentCursor();
            if (cursor == "")
            {
                if (toolAnimator == null)
                {
                    toolAnimator = playerMovement.ToolAnimator();
                }

                toolAnimator.SetTrigger("Hit");
            }
        }
        if (onFoot.Close.triggered)
        {
            CursorController.instance.ChangeCursor("", null);
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
