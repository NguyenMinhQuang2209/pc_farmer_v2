using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] private float maxFood = 100f;
    [SerializeField] private float plusFoodRate = 1f;
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
}
