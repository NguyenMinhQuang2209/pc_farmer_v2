using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform parent;
    private Rigidbody2D rb;
    private float damage = 0;

    public void BulletInit(Transform parent, Vector2 dir, float speed, float damage, float bulletDelayDieTime)
    {
        rb = GetComponent<Rigidbody2D>();
        this.damage = damage;
        this.parent = parent;

        rb.AddForce(dir * speed, ForceMode2D.Impulse);

        Destroy(gameObject, bulletDelayDieTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != parent)
        {
            if (collision.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(damage, parent.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
