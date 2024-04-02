using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth = 0;
    protected int plusHealth = 0;
    bool isDealth = false;
    public void HealthInit()
    {
        currentHealth = maxHealth + plusHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDealth)
        {
            return;
        }
        currentHealth = Mathf.Max(0, currentHealth - damage);
        if (currentHealth == 0)
        {
            Dealth();
        }
    }
    public virtual void TakeDamage(int damage, GameObject target)
    {
        TakeDamage(damage);
    }

    public virtual void Dealth()
    {
        isDealth = true;
    }
    public int GetMaxHealth()
    {
        return maxHealth + plusHealth;
    }
    public void RecoverHealth(int health)
    {
        currentHealth = Mathf.Min(currentHealth + health, GetMaxHealth());
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
