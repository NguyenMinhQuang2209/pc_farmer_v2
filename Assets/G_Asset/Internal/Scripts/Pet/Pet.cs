using System.Collections;
using System.Collections.Generic;
using System.Text;
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

    [Space(5)]
    [Header("Pet upgrade")]
    [SerializeField] private string pet_name = "";
    [SerializeField] private int maxFood = 100;
    int plusFood = 0;
    int currentFood = 0;
    [SerializeField] private int near_damage = 1;
    int plusNearDamage = 0;
    [SerializeField] private int far_damage = 1;
    int plusFarDamage = 0;
    [SerializeField] private float timeBwtAttack = 1f;
    float plusTimeBwtAttack = 0f;

    float plusPatrolDistance = 0f;
    [SerializeField] private int recoverHealthPricePerUnit = 1;
    [SerializeField] private float firstExe = 10f;
    [SerializeField] private float nextLevelRate = 1.2f;
    public Sprite mainSprite;

    [SerializeField] private List<Pet_Level_Item> levels = new();
    int current = 0;

    float currentExe = 0f;
    float nextExe = 0f;

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

        nextExe = firstExe;
        currentExe = 0f;

        HealthInit();
    }
    private void Update()
    {
        if (player == null)
        {
            player = PreferenceController.instance.player;
        }
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
    public string PetName()
    {
        return pet_name;
    }

    public string GetPetMode()
    {
        switch (petMode)
        {
            case PetMode.Protect:
                return "Bảo vệ";
            case PetMode.StayInPosition:
                return "Đứng im";
            case PetMode.Patrol:
                return "Tuần tra";
            case PetMode.Follow:
                return "Đi theo";
        }
        return "";
    }
    public Sprite GetSprite()
    {
        return mainSprite;
    }
    public int GetCurrentFood()
    {
        return currentFood;
    }
    public int GetMaxFood()
    {
        return maxFood + plusFood;
    }
    public float GetMaxTimeBwtAttack()
    {
        return timeBwtAttack + plusTimeBwtAttack;
    }
    public float GetPatrolDistance()
    {
        return radiousCheck + plusPatrolDistance;
    }

    public int RecoverCoin()
    {
        return RecoverCoin(GetMaxHealth() - GetCurrentHealth());
    }

    public int RecoverCoin(int v)
    {
        return recoverHealthPricePerUnit * v;
    }
    public int GetNearDamage()
    {
        return near_damage + plusNearDamage;
    }
    public int GetFarDamage()
    {
        return far_damage + plusFarDamage;
    }
    public string GetPetDetail()
    {
        StringBuilder builder = new();

        builder.Append("Sức mạnh đánh gần: ").Append(GetNearDamage()).AppendLine();
        builder.Append("Sức mạnh đánh xa: ").Append(GetFarDamage()).AppendLine();
        builder.Append("Đói: ").Append(currentFood).Append("/").Append(GetMaxFood()).AppendLine();
        builder.Append("Máu: ").Append(GetCurrentHealth()).Append("/").Append(GetMaxHealth()).AppendLine();
        builder.Append("Thời gian tấn công: ").Append(GetMaxTimeBwtAttack()).AppendLine();
        builder.Append("Khoảng cách tuần tra: ").Append(GetPatrolDistance()).AppendLine();

        return builder.ToString();
    }
    public string GetLevel()
    {
        return current < levels.Count ? (current + 1).ToString() : "Max";
    }

    public bool IsMaxLevel()
    {
        return current < levels.Count;
    }
    public void Upgrade()
    {
        if (IsMaxLevel())
        {
            LogController.instance.Log("Is Max level now");
            return;
        }
        current += 1;
        int current_level = current < levels.Count ? (current + 1) : levels.Count;
        nextExe = firstExe * current_level * nextLevelRate;

        LoadUpgradeDetail();
    }
    public void AddExe(float v)
    {
        currentExe += v;
        if (currentExe >= nextExe)
        {
            Upgrade();
        }
    }
    public float GetCurrentExe()
    {
        return currentExe;
    }
    public float GetNextExe()
    {
        return nextExe;
    }

    public int GetNextPrice()
    {
        return levels[current].price;
    }

    public void LoadUpgradeDetail()
    {
        if (current == 0)
        {
            return;
        }

        int p_nearDamage = 0;
        int p_farDamage = 0;
        int p_food = 0;
        float p_timeBwtAttack = 0;
        int p_health = 0;
        float p_patrolDistance = 0f;
        int current_level = current < levels.Count ? current : levels.Count;

        for (int i = 0; i < current_level; i++)
        {
            Pet_Level_Item item = levels[current];
            for (int j = 0; j < item.upgrades.Count; j++)
            {
                Pet_Level_Item_Upgrade upgradeItem = item.upgrades[j];
                switch (upgradeItem.upgradeName)
                {
                    case UpgradeName.NearDamage:
                        p_nearDamage += (int)upgradeItem.updateValue;
                        break;
                    case UpgradeName.FarDamage:
                        p_farDamage += (int)upgradeItem.updateValue;
                        break;
                    case UpgradeName.Food:
                        p_food += (int)upgradeItem.updateValue;
                        break;
                    case UpgradeName.TimeBwtAttack:
                        p_timeBwtAttack += upgradeItem.updateValue;
                        break;
                    case UpgradeName.Health:
                        p_health += (int)upgradeItem.updateValue;
                        break;
                    case UpgradeName.PatrolDistance:
                        p_patrolDistance += upgradeItem.updateValue;
                        break;
                }
            }
        }

        plusFarDamage = p_farDamage;
        plusNearDamage = p_nearDamage;
        plusFood = p_food;
        plusTimeBwtAttack = p_timeBwtAttack;
        plusPatrolDistance = p_patrolDistance;
        plusHealth = p_health;
    }

}
[System.Serializable]
public class Pet_Level_Item
{
    public int price = 1;
    public List<Pet_Level_Item_Upgrade> upgrades = new();
}
[System.Serializable]
public class Pet_Level_Item_Upgrade
{
    public UpgradeName upgradeName;
    public float updateValue = 1;
}