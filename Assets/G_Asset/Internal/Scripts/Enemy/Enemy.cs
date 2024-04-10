using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Health
{

    public static string ANI_ATTACK = "Attack";
    public static string ANI_ATTACK_INDEX = "AttackIndex";

    [Header("Default")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float totalExe = 100f;


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

    [Header("Attack")]
    [SerializeField] private float sawDistance = 10f;
    [SerializeField] private float stopChasingDistance = 15f;

    [Header("Attack (Near Attack)")]
    [SerializeField] private LayerMask attackMask;
    [SerializeField] private float nearStopAttackDistance = 0.1f;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRadious = 0.1f;
    [SerializeField] private float nearTimeBwtAttack = 1f;
    [SerializeField] private float nearAttackDamage = 1f;
    [Space(5)]
    [Header("Attack Type")]
    [SerializeField] private bool useNearAttack = false;
    [SerializeField] private Vector2Int nearAttackIndex = Vector2Int.zero;
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform shootPlace;
    [SerializeField] private Vector2Int farAttackIndex = Vector2Int.zero;
    [SerializeField] private float farAttackDistance = 1f;
    [SerializeField] private float farAttackTimeBwtAttack = 1f;
    [SerializeField] private float farAttackDamage = 1f;
    [SerializeField] private float farAttackSpeed = 1f;
    [SerializeField] private float farAttackDelay = 1f;

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

        if (target != null)
        {
            Vector2 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (shootPlace != null)
            {
                shootPlace.transform.rotation = Quaternion.Euler(new(0f, 0f, angle - 90f));
            }
        }
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
    public float GetExe(float damage)
    {
        return damage * totalExe / GetMaxHealth();
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
    public float GetFarAttackDistance()
    {
        return farAttackDistance;
    }
    public void FarAttack()
    {
        int attackIndex = Random.Range(farAttackIndex.x, farAttackIndex.y + 1);
        animator.SetFloat(ANI_ATTACK_INDEX, attackIndex);
        animator.SetTrigger(ANI_ATTACK);
        Bullet tempBullet = Instantiate(bullet, shootPlace.transform.position, Quaternion.identity);
        tempBullet.BulletInit(transform, shootPlace.transform.up, farAttackSpeed, farAttackDamage, farAttackDelay);
    }
    public float GetTimeBwtAttackFarAttack()
    {
        return farAttackTimeBwtAttack;
    }
    public bool UseNearAttack()
    {
        return useNearAttack;
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
    public float GetNearStopAttackDistance()
    {
        return nearStopAttackDistance;
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
    public override void TakeDamage(float damage)
    {
        animator.SetTrigger("Hit");
        base.TakeDamage(damage);
    }
    public float GetNearTimeBwtAttack()
    {
        return nearTimeBwtAttack;
    }
    public override void TakeDamage(float damage, GameObject targetObject)
    {
        int layer = targetObject.layer;
        bool isObjectInLayer = ((1 << layer) & attackMask) != 0;
        if (target == null)
        {
            if (isObjectInLayer)
            {
                if (targetObject.TryGetComponent<Health>(out target))
                {
                    LevelController.instance.AddExeTotal(GetExe(damage));
                }
            }
        }
        else
        {
            if (isObjectInLayer)
            {
                LevelController.instance.AddExeTotal(GetExe(damage));
            }
        }
        TakeDamage(damage);
    }
    public void Attack()
    {
        int attackIndex = Random.Range(nearAttackIndex.x, nearAttackIndex.y + 1);
        animator.SetFloat(ANI_ATTACK_INDEX, attackIndex);
        animator.SetTrigger(ANI_ATTACK);
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPos.position, attackRadious, attackMask);
        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D hit = hits[i];
            if (hit.gameObject != gameObject)
            {
                if (hit.gameObject.TryGetComponent<Health>(out var health))
                {
                    health.TakeDamage(nearAttackDamage);
                }
            }
        }
    }
    public void OnDrawGizmos()
    {
        if (attackPos != null)
        {
            Gizmos.DrawWireSphere(attackPos.position, attackRadious);
        }
    }
    public bool UseFarAttack()
    {
        return bullet != null;
    }
}
