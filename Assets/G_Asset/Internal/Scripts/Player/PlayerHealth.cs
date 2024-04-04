using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] private float maxFood = 100f;
    [SerializeField] private float plusFoodRate = 1f;

    [SerializeField] private float targetFoodReduce = 1f;
    [SerializeField] private float reduceFoodRate = 1f;
    float plusFoodReduceTimer = 0f;
    float currentFoodReduce = 0f;
    int plusFood = 0;
    float currentFood = 0f;
    private PlayerMovement playerMovement;
    private void Start()
    {
        HealthInit();
        playerMovement = GetComponent<PlayerMovement>();
        currentFood = GetMaxFood();
    }
    private void Update()
    {
        RecoverHealthInit();

        ComsumeFood();

    }

    public void ComsumeFood()
    {
        if (currentFood > 0f)
        {
            currentFoodReduce += Time.deltaTime;
            if (currentFoodReduce >= targetFoodReduce + plusFoodReduceTimer)
            {
                currentFoodReduce = targetFoodReduce + plusFoodReduceTimer;
                UseFood();
            }
        }
    }
    public void UseFood()
    {
        currentFood = Mathf.Max(0f, currentFood - Time.deltaTime * reduceFoodRate);
        if (currentFood == 0f)
        {
            currentFoodReduce = 0f;
        }
    }
    public float GetMaxFood()
    {
        return Mathf.Ceil(maxFood + plusFood * plusFoodRate);
    }
    public float GetCurrentFood()
    {
        return Mathf.Ceil(currentFood);
    }
    public float GetSpeed()
    {
        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }
        return playerMovement.GetSpeed();
    }

    public void ChangePlus(int plusHealth, int plusFood, int plusSpeed, int plusRecoverRate)
    {
        this.plusFood = plusFood;
        plusRecoverHealth = plusRecoverRate;
        this.plusHealth = plusHealth;
        playerMovement.ChangeSpeedPlus(plusSpeed);
    }
    public override void RecoverHealthInit()
    {
        if (GetCurrentFood() == 0f)
        {
            ResetTime();
            return;
        }
        base.RecoverHealthInit();
    }
    public void ChangePlusReduceTimer(float newTime, float duration)
    {
        currentFoodReduce = 0f;
        plusFoodReduceTimer = newTime;
        Invoke(nameof(ResetPlusTime), duration);
    }

    public void ChangePlusReduceTimer(float newTime)
    {
        currentFoodReduce = 0f;
        plusFoodReduceTimer = newTime;
    }
    public void Feed(float v)
    {
        currentFood = Mathf.Min(currentFood + v, GetMaxFood());
        currentFoodReduce = 0f;
    }
    public void Feed(float v, float newTime)
    {
        currentFood = Mathf.Min(currentFood + v, GetMaxFood());
        ChangePlusReduceTimer(newTime);
    }
    public void Feed(float v, float newTime, float duration)
    {
        currentFood = Mathf.Min(currentFood + v, GetMaxFood());
        ChangePlusReduceTimer(newTime, duration);
    }

    private void ResetPlusTime()
    {
        plusFoodReduceTimer = 0f;
    }
}
