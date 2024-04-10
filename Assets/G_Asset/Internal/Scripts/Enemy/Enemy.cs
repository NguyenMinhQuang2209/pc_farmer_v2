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
    [SerializeField] private float stopAttackDistance = 0.1f;
    [SerializeField] private float sawDistance = 10f;

    [SerializeField] private float stopChasingDistance = 15f;

    [Header("Patrol")]
    [SerializeField] private Vector2 wanderX = Vector2.zero;
    [SerializeField] private float stopDistance = 0.1f;
    [SerializeField] private Vector2 wanderY = Vector2.zero;
    [SerializeField] private float waitTimer = 10f;

    private NavMeshAgent agent;
    private Animator animator;
    private State currentState;

    private Health target = null;
    private Vector3 defaultPosition = new();
    [SerializeField] private bool patrolInPosition = false;
    [Header("Run away state")]
    [SerializeField] private bool useRunAway = false;

    [SerializeField][Range(0f, 1f)] private float runAwayHealthRate = 0f;
    [SerializeField] private float runAwayTimer = 10f;

    [SerializeField] private string currentStateName = "";

    public enum StateName
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
        currentStateName = "Patrol";
    }
    private void Update()
    {
        currentState.Perform();
        RecoverHealthInit();
    }
    public void ChangeState(State newState, string newStateName)
    {
        currentStateName = newStateName;
        currentState.Exit();
        currentState = newState;
        currentState.Enter(this);
    }
    public override void Dealth()
    {
        animator.SetTrigger("Dead");
    }

    public bool CanSeeTarget()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, sawDistance, attackMask);
        if (hit != null)
        {
            if (hit.gameObject.TryGetComponent<Health>(out target))
            {
                return true;
            }
        }
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
    public float GetStopChasingDistance()
    {
        return stopChasingDistance;
    }
    public Health GetTarget()
    {
        return target;
    }
    public Animator GetAnimator()
    {
        return animator;
    }
    public State SawTargetState()
    {
        return new AttackState();
    }
    public void StopChasing()
    {
        target = null;
    }
    public float GetStopAttackDistance()
    {
        return stopAttackDistance;
    }
    public bool RunAway()
    {
        float healthRate = GetCurrentHealth() / GetMaxHealth();
        return useRunAway && healthRate <= runAwayHealthRate;
    }
    public float RunAwayTimer()
    {
        return runAwayTimer;
    }
}
