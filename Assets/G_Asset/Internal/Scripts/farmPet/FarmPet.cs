using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarmPet : MonoBehaviour
{
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
    }
    public void RandomPosition()
    {
        float posX = Random.Range(randomX.x, randomX.y);
        float posY = Random.Range(randomY.x, randomY.y);
        Vector3 newPos = new(nest.position.x + posX, nest.position.y + posY, 0f);
        transform.rotation = Quaternion.Euler(new(0f, newPos.x > transform.position.x ? 0f : 180f, 0f));

        agent.SetDestination(newPos);
    }
}
