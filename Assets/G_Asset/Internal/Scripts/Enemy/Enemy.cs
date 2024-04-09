using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Health
{
    [Header("Default")]
    [SerializeField] private float moveSpeed = 1f;

    [Header("Attack")]
    [SerializeField] private LayerMask attackMask;
    [SerializeField] private float stopDistance = 0.1f;
    [SerializeField] private float sawDistance = 10f;

    [SerializeField] private float stopChasingDistance = 15f;

    [Header("Patrol")]
    [SerializeField] private Vector2 wanderX = Vector2.zero;
    [SerializeField] private Vector2 wanderY = Vector2.zero;
    [SerializeField] private float waitTimer = 10f;

    private NavMeshAgent agent;
    private Animator animator;
    private State currentState;

    private GameObject target = null;
    private Vector3 defaultPosition = new();
    [SerializeField] private bool patrolInPosition = false;

    public enum EnemyState
    {
        Patrol,
        Attack,
        Hide
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
        HealthInit();
        currentState = new PatrolState();
        currentState.Enter(this);
        defaultPosition = transform.position;
    }
    private void Update()
    {
        currentState.Perform();
    }
    public void ChangeState(State newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter(this);
    }

    public bool CanSeeTarget()
    {

        return false;
    }
    public float GetWaitTime()
    {
        return waitTimer;
    }
    public NavMeshAgent GetAgent()
    {
        return agent;
    }
    public Vector3 GetNewPosition()
    {
        Vector3 newPos = patrolInPosition ? defaultPosition : transform.position;

        float ranX = Random.Range(wanderX.x, wanderX.y);
        float ranY = Random.Range(wanderY.x, wanderY.y);

        Vector3 targetPos = new(newPos.x + ranX, newPos.y + ranY, newPos.z);
        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
        {
            return targetPos;
        }
        return newPos;
    }
    public float GetStopDistance()
    {
        return stopDistance;
    }
    public Animator GetAnimator()
    {
        return animator;
    }
}
