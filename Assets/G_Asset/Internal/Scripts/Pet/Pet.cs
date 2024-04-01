using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pet : Health
{
    private NavMeshAgent agent;

    public static string ATTACK = "Attack";
    public static string ATTACK_INDEX = "Attack_index";
    public static string HURT = "Hurt";

    [SerializeField] private float moveSpeed = 0.15f;
    [SerializeField] private float runSpeed = 0.2f;
    [SerializeField] private float stopDistance = 0.2f;
    [SerializeField] private float telepoteDistance = 10f;

    [SerializeField] private float radiousCheck = 0.5f;
    [SerializeField] private GameObject check;

    [SerializeField] private PetMode petMode;

    [Header("Attack")]
    [SerializeField] private float timeBwtAttack = 1f;
    [SerializeField] private int maxAttackIndex = 1;
    [SerializeField] private LayerMask enemyMask;
    float currentTimeBwtAttack = 0f;

    [Header("Wander")]
    [SerializeField] private float wanderTime = 1f;
    [SerializeField] private Vector2 wanderXAxis = Vector2.one;
    [SerializeField] private Vector2 wanderYAxis = Vector2.one;
    float currentWanderTime = 0f;

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;

    private Vector3 watchPosition;
    private Vector3 target;
    private Transform enemy;
    private void Start()
    {
        /*agent = GetComponent<NavMeshAgent>();

        agent.speed = moveSpeed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.enabled = false;*/

        currentTimeBwtAttack = timeBwtAttack;
        currentWanderTime = wanderTime;

        player = PreferenceController.instance.player;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        check.SetActive(false);
        check.transform.localScale = new(radiousCheck, radiousCheck, 1);
    }
    private void Update()
    {
        currentTimeBwtAttack = Mathf.Min(currentTimeBwtAttack + Time.deltaTime, timeBwtAttack);
        StateMachine();
    }


    public void ChangePetMode(PetMode newMode)
    {
        //agent.isStopped = true;
        watchPosition = transform.position;
        rb.velocity = new(0f, 0f);
        enemy = null;
        petMode = newMode;
    }

    private void CheckPetMode()
    {
        switch (petMode)
        {
            case PetMode.Protect:
                Protect();
                break;
            case PetMode.Follow:
                Follow();
                break;
            case PetMode.StayInPosition:
                StayInPosition();
                break;
            case PetMode.Patrol:
                Watch();
                break;
            default:
                //agent.isStopped = true;
                break;
        }
    }
    private void Protect()
    {
        if (enemy == null)
        {
            Follow();
            FindEnemy();
        }
        else
        {
            AttackState();
        }

    }
    private void Follow()
    {
        float distance = Vector2.Distance(player.position, transform.position);

        float yAxis = player.position.x - transform.position.x;
        transform.rotation = Quaternion.Euler(new(0f, yAxis < 0 ? 180f : 0f, 0f));

        bool isStop = false;

        if (distance > telepoteDistance)
        {
            transform.position = player.transform.position;
            return;
        }

        if (distance > stopDistance)
        {
            /* if (agent.isStopped)
             {
                 agent.isStopped = false;
             }
             agent.SetDestination(player.position);*/
            Vector2 targetDir = player.position - transform.position;
            targetDir.Normalize();

            rb.velocity = targetDir * moveSpeed;
        }
        else
        {
            //agent.SetDestination(transform.position);
            rb.velocity = new(0f, 0f);
            isStop = true;
        }

        animator.SetFloat("Speed", isStop ? 0f : 1f);
    }
    private void StayInPosition()
    {
        /*if (!agent.isStopped)
        {
            agent.isStopped = true;
        }*/

        if (enemy == null)
        {
            FindEnemy();
        }
        else
        {
            AttackState();
        }
    }
    private void Watch()
    {
        /*if (agent.isStopped)
        {
            agent.isStopped = false;
        }*/


        if (enemy == null)
        {
            bool isStop = false;
            float distance = Vector2.Distance(target, transform.position);
            if (distance >= 0.1f)
            {
                Vector2 dir = target - transform.position;
                dir.Normalize();

                rb.velocity = dir * moveSpeed;

                float yAxis = target.x - transform.position.x;
                transform.rotation = Quaternion.Euler(new(0f, yAxis < 0 ? 180f : 0f, 0f));
                currentWanderTime = 0f;
            }
            else
            {
                currentWanderTime = Mathf.Min(currentWanderTime + Time.deltaTime, wanderTime);
                if (currentWanderTime >= wanderTime)
                {
                    float ranX = Random.Range(wanderXAxis.x, wanderXAxis.y);
                    float ranY = Random.Range(wanderYAxis.x, wanderYAxis.y);
                    currentWanderTime = 0f;

                    target = new(watchPosition.x + ranX, watchPosition.y + ranY);
                }
                else
                {
                    isStop = true;
                    rb.velocity = new(0f, 0f);
                }
            }
            animator.SetFloat("Speed", isStop ? 0f : 1f);

            FindEnemy();
        }
        else
        {
            AttackState();
        }

    }
    public void StateMachine()
    {
        CheckPetMode();
    }
    public void FindEnemy()
    {
        Collider2D hits = Physics2D.OverlapCircle(check.transform.position, radiousCheck, enemyMask);
        if (hits != null)
        {
            enemy = hits.transform;
        }
    }

    public void AttackState()
    {
        bool isStop = false;
        float distance = Vector2.Distance(enemy.position, transform.position);
        float yAxis = enemy.position.x - transform.position.x;
        transform.rotation = Quaternion.Euler(new(0f, yAxis < 0 ? 180f : 0f, 0f));
        if (distance > stopDistance)
        {
            Vector2 targetDir = enemy.position - transform.position;
            rb.velocity = targetDir * runSpeed;
        }
        else
        {
            PlayerAttack();
            rb.velocity = new(0f, 0f);
            isStop = true;
        }
        animator.SetFloat("Speed", isStop ? 0f : 2f);
    }
    private void PlayerAttack()
    {
        currentTimeBwtAttack = Mathf.Min(currentTimeBwtAttack + Time.deltaTime, timeBwtAttack);
        if (currentTimeBwtAttack >= timeBwtAttack)
        {
            currentTimeBwtAttack = 0f;

            int next = Random.Range(0, maxAttackIndex);
            animator.SetFloat(ATTACK_INDEX, next);
            animator.SetTrigger(ATTACK);
        }
    }

    public override void TakeDamage(int damage)
    {
        animator.SetTrigger(HURT);
        base.TakeDamage(damage);
    }

}
