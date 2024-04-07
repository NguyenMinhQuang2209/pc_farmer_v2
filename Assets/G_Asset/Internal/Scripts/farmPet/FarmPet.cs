using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarmPet : MonoBehaviour
{
    [Header("Pet item init")]
    public Sprite img;
    [HideInInspector] public string petName = "";
    [SerializeField] private List<PetFarmGrow> petFarmGrows = new();
    public int currentIndex = 0;
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

    [SerializeField] private NestName nestName;
    private Transform nest;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = 0.1f;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator = GetComponent<Animator>();
        currentWaitTimer = waitTimer;

        nest = PreferenceController.instance.GetNest(nestName);
        targetTime = 0f;
        currentIndex = 0;
        for (int i = 0; i < petFarmGrows.Count; i++)
        {
            PetFarmGrow petItem = petFarmGrows[i];
            petName = petItem.petName;
            targetTime += petItem.growingTime;
            transform.localScale = petItem.size;
        }
        targetChangeTime = petFarmGrows[currentIndex].growingTime;

    }
    private void Update()
    {
        float speed = moveSpeed;
        if (agent.remainingDistance <= 0.05f)
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
    }
    public void RandomPosition()
    {
        float posX = Random.Range(randomX.x, randomX.y);
        float posY = Random.Range(randomY.x, randomY.y);
        Vector3 newPos = new(nest.position.x + posX, nest.position.y + posY, 0f);
        transform.rotation = Quaternion.Euler(new(0f, newPos.x > transform.position.x ? 0f : 180f, 0f));

        agent.SetDestination(newPos);
    }
    public float GetGrowingTime()
    {
        return currentGrowingTime;
    }
    public string GetGrowingRemainTime()
    {
        return Mathf.Ceil(targetTime - currentGrowingTime) + "s";
    }
    public void GrowingTime()
    {
        currentGrowingTime = Mathf.Min(currentGrowingTime + Time.deltaTime, targetTime);
        if (currentGrowingTime < targetTime)
        {
            if (currentGrowingTime >= targetChangeTime)
            {
                currentIndex += 1;
                if (currentIndex < petFarmGrows.Count)
                {
                    PetFarmGrow petItem = petFarmGrows[currentIndex];
                    petName = petItem.petName;
                    transform.localScale = petItem.size;
                    targetChangeTime += petFarmGrows[currentIndex].growingTime;
                }
            }
        }
        else
        {
            if (currentIndex != petFarmGrows.Count - 1)
            {
                currentIndex = petFarmGrows.Count - 1;
                PetFarmGrow petItem = petFarmGrows[currentIndex];
                petName = petItem.petName;
                transform.localScale = petItem.size;
            }
        }
    }
}
[System.Serializable]
public class PetFarmGrow
{
    public Vector3 size = new();
    public string petName = "";
    public float growingTime = 0f;
}