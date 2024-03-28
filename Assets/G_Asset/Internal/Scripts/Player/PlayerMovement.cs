using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 1f;
    private Animator animator;
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
    }
    public void Movement(Vector2 dir)
    {
        if (dir.sqrMagnitude >= 0.1f)
        {

            int nextIdle_state = 0;

            if (dir.x > 0f)
            {
                nextIdle_state = 3;
            }
            else if (dir.x < 0f)
            {
                nextIdle_state = 2;
            }

            if (dir.y > 0f)
            {
                nextIdle_state = 1;
            }
            else if (dir.y < 0f)
            {
                nextIdle_state = 0;
            }

            animator.SetFloat("Horizontal", dir.x);
            animator.SetFloat("Vertical", dir.y);
            animator.SetFloat("Idle_State", nextIdle_state);
            rb.MovePosition(rb.position + moveSpeed * dir * Time.deltaTime);
        }
        animator.SetFloat("Speed", dir.sqrMagnitude >= 0.1f ? 1f : 0f);
    }
}
