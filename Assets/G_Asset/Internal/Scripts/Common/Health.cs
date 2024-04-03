using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float plusHealthRate = 1;
    [SerializeField] private float currentHealth = 0;
    protected int plusHealth = 0;
    protected int plusRecoverHealth = 0;
    bool isDealth = false;

    [Header("Recover config")]
    [SerializeField] protected float recoverHealthDefault = 1f;
    [SerializeField] protected float recoverHealthRate = 1f;
    [SerializeField] protected float waitToRecoverTimer = 2f;
    float currentRecoverTimer = 0f;

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
        currentRecoverTimer = 0f;
        TakeDamage(damage);
    }

    public void RecoverHealthInit()
    {
        if (currentHealth < GetMaxHealth())
        {
            currentRecoverTimer += Time.deltaTime;
            if (currentRecoverTimer >= waitToRecoverTimer)
            {
                float hp = GetRecoverRate() * Time.deltaTime;
                RecoverHealth(hp);
            }
        }
    }

    public virtual void Dealth()
    {
        isDealth = true;
    }
    public float GetMaxHealth()
    {
        return maxHealth + plusHealth * plusHealthRate;
    }
    public void RecoverHealth(float health)
    {
        currentHealth = Mathf.Min(currentHealth + health, GetMaxHealth());
    }
    public void RecoverAllHealth()
    {
        currentHealth = GetMaxHealth();
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetRecoverRate()
    {
        return recoverHealthDefault + plusRecoverHealth * recoverHealthRate;
    }
}
