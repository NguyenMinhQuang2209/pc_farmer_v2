using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : MonoBehaviour
{
    [SerializeField] protected float maxFood = 100f;
    [SerializeField] protected float reduceFoodRate = 1f;
    [SerializeField] protected float waitFoodTimer = 10f;
    protected float currentWaitFoodTimer = 0f;
    protected float currentFood = 0f;
    public void FoodInit()
    {
        currentFood = maxFood;
    }
    public float GetMaxFood()
    {
        return maxFood;
    }
    public void UseFood()
    {
        currentWaitFoodTimer = Mathf.Min(currentWaitFoodTimer + Time.deltaTime, waitFoodTimer);
        if (currentWaitFoodTimer == waitFoodTimer)
        {
            currentFood = Mathf.Max(currentFood - Time.deltaTime * reduceFoodRate, 0f);
        }
    }
    public void RecoverFood(float v)
    {
        currentFood = Mathf.Min(currentFood + v, maxFood);
        currentWaitFoodTimer = 0f;
    }
}
