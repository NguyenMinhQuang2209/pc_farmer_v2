using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarmPet : Food
{
    [Header("Pet item init")]
    public Sprite img;
    public string petName = "";
    [SerializeField] private List<PetFarmGrow> petFarmGrows = new();
    private int currentIndex = 0;
    float targetTime = 0f;
    float targetChangeTime = 0f;
    float currentGrowingTime = 0f;

    [Space(5)]
    [Header("Pet world config")]
    private NavMeshAgent agent;
    [SerializeField] private float moveSpeed = 1f;
    private Animator animator;
    [SerializeField] private float waitTimer = 10f;
    float currentWaitTimer = 0f;
    [SerializeField] private Vector2 randomX = Vector2.zero;
    [SerializeField] private Vector2 randomY = Vector2.zero;
    private Transform nest;

    private Interactible interact;

    private int currentPrice = 0;

    bool isEndGrowing = false;

    [Space(5)]
    [Header("Pet sell")]
    [SerializeField] private int sellPrice = 20;

    [Space(5)]
    [Header("Produce product")]
    [SerializeField] private Vector2Int quantity = Vector2Int.zero;
    [SerializeField] private float waitProduceTimer = 1f;
    [SerializeField] private Vector2Int producePerTime = Vector2Int.zero;
    float currentWaitProduceTimer = 0f;
    int currentQuantity = 0;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        interact = GetComponent<Interactible>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = 0.1f;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator = GetComponent<Animator>();
        currentWaitTimer = waitTimer;
        targetTime = GetTotalTime();
        currentIndex = 0;
        if (petFarmGrows.Count > 0)
        {
            petName = petFarmGrows[currentIndex].petName;
            transform.localScale = petFarmGrows[currentIndex].size;
            targetChangeTime = petFarmGrows[currentIndex].growingTime;
            currentPrice = (int)Mathf.Ceil(petFarmGrows[currentIndex].priceRate * sellPrice);
            isEndGrowing = false;
        }
        else
        {
            isEndGrowing = true;
        }
        if (interact != null)
        {
            interact.promptMessage = petName;
        }

        int maxQuantity = Mathf.Max(quantity.x, quantity.y);
        int minQuantity = Mathf.Min(quantity.x, quantity.y);

        maxQuantity = maxQuantity > 0 ? maxQuantity : 0;
        minQuantity = minQuantity > 0 ? minQuantity : 0;

        currentQuantity = Random.Range(minQuantity, maxQuantity + 1);

        FoodInit();
    }
    public void Produce()
    {
        if (isEndGrowing && currentQuantity > 0)
        {
            currentWaitProduceTimer = Mathf.Min(currentWaitProduceTimer + Time.deltaTime, waitProduceTimer);
            if (currentWaitProduceTimer == waitProduceTimer)
            {
                currentWaitProduceTimer = 0f;
                ProduceProduct();
            }
        }
    }
    public void ProduceProduct()
    {
        int perTime = Random.Range(Mathf.Min(producePerTime.x, producePerTime.y), Mathf.Max(producePerTime.x, producePerTime.y) + 1);

        int finalQuantity = currentQuantity > perTime ? perTime : currentQuantity;

        currentQuantity -= finalQuantity;

        if (nest.gameObject.TryGetComponent<Nest>(out var nestItem))
        {
            nestItem.ProduceProduct(finalQuantity);
        }
    }
    public void FarmPetInit(Transform nest)
    {
        this.nest = nest;
    }
    private void Update()
    {
        if (nest == null)
        {
            return;
        }
        float speed = moveSpeed;
        if (agent.remainingDistance <= 0.1f)
        {
            currentWaitTimer += Time.deltaTime;
            if (currentWaitTimer >= waitTimer)
            {
                currentWaitTimer = 0f;
                RandomPosition();
            }
            speed = 0f;
        }
        animator.SetFloat("Speed", speed);

        GrowingTime();
        UseFood();
        Produce();
    }
    public void RandomPosition()
    {
        float posX = Random.Range(randomX.x, randomX.y);
        float posY = Random.Range(randomY.x, randomY.y);
        Vector3 newPos = new(nest.position.x + posX, nest.position.y + posY, 0f);
        transform.rotation = Quaternion.Euler(new(0f, newPos.x > transform.position.x ? 0f : 180f, 0f));

        if (!NavMesh.SamplePosition(newPos, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
        {
            newPos = transform.position;
        }

        agent.SetDestination(newPos);
    }
    public float GetCurrentFood()
    {
        return currentFood;
    }
    public float GetGrowingTime()
    {
        return currentGrowingTime;
    }
    public int GetCurrentPrice()
    {
        return currentPrice;
    }
    public float GetTotalTime()
    {
        float totalTime = 0;
        for (int i = 0; i < petFarmGrows.Count; i++)
        {
            PetFarmGrow petItem = petFarmGrows[i];
            totalTime += petItem.growingTime;
        }
        return totalTime;
    }
    public string GetGrowingRemainTime()
    {
        return Mathf.Ceil(GetTotalTime() - currentGrowingTime) + "s";
    }
    public void GrowingTime()
    {
        if (currentFood == 0f || isEndGrowing)
        {
            return;
        }

        currentGrowingTime = Mathf.Min(currentGrowingTime + Time.deltaTime, targetTime);
        if (currentGrowingTime < targetTime)
        {
            if (currentGrowingTime >= targetChangeTime)
            {
                currentIndex += 1;
                if (currentIndex < petFarmGrows.Count)
                {
                    targetChangeTime += petFarmGrows[currentIndex].growingTime;
                    currentPrice = (int)Mathf.Ceil(petFarmGrows[currentIndex].priceRate * sellPrice);
                }
            }
        }
        else
        {
            if (currentIndex != petFarmGrows.Count - 1)
            {
                currentIndex = petFarmGrows.Count - 1;
                isEndGrowing = true;
                currentPrice = sellPrice;
            }
        }
        if (interact != null)
        {
            interact.promptMessage = petName;
        }
        PetFarmGrow petItem = petFarmGrows[currentIndex];
        petName = petItem.petName;
        transform.localScale = petItem.size;

    }
    public void Feed(float v)
    {
        RecoverFood(v);
    }
}
[System.Serializable]
public class PetFarmGrow
{
    public Vector3 size = new();
    public string petName = "";
    public float growingTime = 0f;
    public float priceRate = 0.1f;
}