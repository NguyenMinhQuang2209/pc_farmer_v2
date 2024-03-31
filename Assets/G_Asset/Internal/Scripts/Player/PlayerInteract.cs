using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private InteractTarget interactTarget;
    private PlayerInput playerInput;

    [SerializeField] private Vector2 offset = new(0.45f, 0.45f);
    private Vector2 interactOffset = Vector2.zero;

    private PlayerToward playerToward = PlayerToward.Bottom;
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.TryGetComponent<InteractTarget>(out interactTarget))
            {
                break;
            }
        }
        playerInput = GetComponent<PlayerInput>();
        interactOffset = new(0f, offset.y);
    }
    private void Update()
    {
        Vector2 targetPosition = new(transform.position.x + interactOffset.x, transform.position.y + interactOffset.y);
        targetPosition.x = Mathf.Floor(targetPosition.x / 0.16f) * 0.16f;
        targetPosition.y = Mathf.Floor(targetPosition.y / 0.16f) * 0.16f;

        if (playerToward == PlayerToward.Left)
        {
            targetPosition.x += 0.16f;
        }
        if (playerToward == PlayerToward.Bottom)
        {
            targetPosition.y += 0.16f;
        }

        interactTarget.transform.position = targetPosition;

        if (playerInput.onFoot.Interact.triggered)
        {
            interactTarget.Interact();
        }

    }
    public void ChangePlayerToward(PlayerToward toward)
    {
        playerToward = toward;
        switch (toward)
        {
            case PlayerToward.Top:
                interactOffset = new(0f, offset.y);
                break;
            case PlayerToward.Bottom:
                interactOffset = new(0f, offset.y * -1f);
                break;
            case PlayerToward.Left:
                interactOffset = new(offset.x * -1f, 0f);
                break;
            case PlayerToward.Right:
                interactOffset = new(offset.x, 0f);
                break;
        }
    }
}
