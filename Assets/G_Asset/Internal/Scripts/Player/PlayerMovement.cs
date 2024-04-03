using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float plusSpeedRate = 0.1f;
    int plusSpeed = 0;
    private Animator animator;
    private Animator toolAnimator;

    private PlayerInteract playerInteract;


    [SerializeField] private Transform tool_store;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.gameObject.TryGetComponent<Animator>(out animator))
            {
                break;
            }
        }
        if (tool_store.GetChild(0).gameObject != null)
        {
            if (tool_store.GetChild(0).gameObject.TryGetComponent<Animator>(out toolAnimator))
            {

            }
        }
        playerInteract = GetComponent<PlayerInteract>();
    }
    public float GetSpeed()
    {
        return moveSpeed + plusSpeed * plusSpeedRate;
    }
    public void ChangeSpeedPlus(int v)
    {
        plusSpeed = v;
    }
    public void Movement(Vector2 dir)
    {
        if (dir.sqrMagnitude >= 0.1f)
        {
            int nextIdle_state = 0;

            float rot = 0f;

            if (dir.x > 0f)
            {
                nextIdle_state = 3;
                playerInteract.ChangePlayerToward(PlayerToward.Right);

            }
            else if (dir.x < 0f)
            {
                nextIdle_state = 2;
                playerInteract.ChangePlayerToward(PlayerToward.Left);
                rot = 180f;
            }

            if (dir.y > 0f)
            {
                nextIdle_state = 1;
                playerInteract.ChangePlayerToward(PlayerToward.Top);
            }
            else if (dir.y < 0f)
            {
                nextIdle_state = 0;
                playerInteract.ChangePlayerToward(PlayerToward.Bottom);
            }

            tool_store.transform.rotation = Quaternion.Euler(0f, rot, 0f);
            animator.SetFloat("Horizontal", dir.x);
            animator.SetFloat("Vertical", dir.y);
            animator.SetFloat("Idle_State", nextIdle_state);
            rb.MovePosition(rb.position + GetSpeed() * dir * Time.deltaTime);
        }
        animator.SetFloat("Speed", dir.sqrMagnitude >= 0.1f ? 1f : 0f);
    }

    public Animator ToolAnimator()
    {
        return toolAnimator;
    }
}
