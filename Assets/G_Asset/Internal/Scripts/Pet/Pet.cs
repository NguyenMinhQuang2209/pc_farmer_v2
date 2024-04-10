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

    [SerializeField] private PetName petName;
    [SerializeField] private float moveSpeed = 0.15f;
    [SerializeField] private float runSpeed = 0.2f;
    [SerializeField] private float stopDistance = 0.2f;
    [SerializeField] private float telepoteDistance = 10f;

    [SerializeField] private float radiousCheck = 0.5f;
    [SerializeField] private GameObject check;

    [SerializeField] private PetMode petMode;

    [Header("Attack")]
    [SerializeField] private bool hasNearAttack = false;
    [SerializeField] private bool useNearAttackInDefault = false;
    private bool nearAttack = false;
    [SerializeField] private Vector2Int nearAttackIndex = new();
    [SerializeField] private float stopChasingDistance = 0.2f;
    [SerializeField] private LayerMask enemyMask;
    float currentTimeBwtAttack = 0f;

    [Space(5)]
    [Header("Far attack")]
    [SerializeField] private bool hasFarAttack = false;
    [SerializeField] private bool useFarAttackInDefault = false;
    private bool farAttack = false;
    [SerializeField] private float farAttackDistance = 0.2f;
    [SerializeField] private Vector2Int farAttackIndex = new();
    [SerializeField] private Bullet bullet;
    [SerializeField] private float bulletSpeed = 2f;
    [SerializeField] private float bulletDelayDieTime = 1f;
    [SerializeField] private Transform bullet_shoot;

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
    [SerializeField] private float maxFood = 100f;
    int plusFood = 0;
    float currentFood = 0f;
    [SerializeField] private float near_damage = 1;
    float plusNearDamage = 0;
    [SerializeField] private float far_damage = 1;
    float plusFarDamage = 0;
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

    [Header("Food")]
    [SerializeField] private float targetFoodReduce = 1f;
    [SerializeField] private float reduceFoodRate = 1f;
    float plusFoodReduceTimer = 0f;
    float currentFoodReduce = 0f;


    [Header("Attack State")]
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRadious = 0.2f;

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
        check.transform.localScale = new(GetPatrolDistance(), GetPatrolDistance(), 1);

        nextExe = firstExe;
        currentExe = 0f;

        HealthInit();

        currentFood = GetMaxFood();

        farAttack = hasFarAttack && useFarAttackInDefault;
        nearAttack = hasNearAttack && useNearAttackInDefault;
    }
    private void Update()
    {
        if (player == null)
        {
            player = PreferenceController.instance.player;
        }
        currentTimeBwtAttack = Mathf.Min(currentTimeBwtAttack + Time.deltaTime, timeBwtAttack);
        StateMachine();

        ComsumeFood();
    }
    public PetName GetPetName()
    {
        return petName;
    }

    public void ChangePetMode(PetMode newMode)
    {
        //agent.isStopped = true;
        watchPosition = transform.position;
        target = transform.position;
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
        Collider2D hits = Physics2D.OverlapCircle(check.transform.position, GetPatrolDistance(), enemyMask);
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

        Vector2 dir = enemy.position - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (bullet_shoot != null)
        {
            bullet_shoot.transform.rotation = Quaternion.Euler(new(0f, 0f, angle - 90f));
        }

        if (distance > stopDistance)
        {
            if (distance > stopChasingDistance)
            {
                enemy = null;
            }
            else
            {
                if (farAttack)
                {
                    if (distance <= farAttackDistance)
                    {
                        PlayerAttack(farAttackIndex, false);
                        rb.velocity = new(0f, 0f);
                        isStop = true;
                    }
                    else
                    {
                        Vector2 targetDir = enemy.position - transform.position;
                        rb.velocity = targetDir * runSpeed;
                    }
                }
                else
                {
                    Vector2 targetDir = enemy.position - transform.position;
                    rb.velocity = targetDir * runSpeed;
                }
            }
        }
        else
        {
            Vector2Int attackIndex;
            if (nearAttack)
            {
                attackIndex = nearAttackIndex;
            }
            else
            {
                attackIndex = farAttackIndex;
            }
            PlayerAttack(attackIndex, attackIndex == nearAttackIndex);
            rb.velocity = new(0f, 0f);
            isStop = true;

        }
        animator.SetFloat("Speed", isStop ? 0f : 2f);
    }
    private void PlayerAttack(Vector2Int attackIndex, bool nearAttack)
    {
        currentTimeBwtAttack = Mathf.Min(currentTimeBwtAttack + Time.deltaTime, timeBwtAttack);
        if (currentTimeBwtAttack >= timeBwtAttack)
        {
            currentTimeBwtAttack = 0f;

            int next = Random.Range(attackIndex.x, attackIndex.y + 1);
            animator.SetFloat(ATTACK_INDEX, next);
            animator.SetTrigger(ATTACK);

            if (!nearAttack)
            {
                Shoot();
            }
            else
            {
                PetAttack();
            }
        }
    }

    public void PetAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPos.position, attackRadious, enemyMask);
        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D hit = hits[i];
            if (hit.gameObject != gameObject)
            {
                if (hit.gameObject.TryGetComponent<Health>(out var health))
                {
                    health.TakeDamage(GetNearDamage(), gameObject);
                }
            }
        }
    }

    public override void TakeDamage(float damage)
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
    public PetMode GetCurrentMode()
    {
        return petMode;
    }
    public Sprite GetSprite()
    {
        return mainSprite;
    }
    public float GetMaxFood()
    {
        return Mathf.Ceil(maxFood + plusFood);
    }
    public float GetCurrentFood()
    {
        return Mathf.Ceil(currentFood);
    }
    public override void RecoverHealthInit()
    {
        if (GetCurrentFood() == 0f)
        {
            ResetTime();
            return;
        }
        base.RecoverHealthInit();
    }
    public void ChangePlusReduceTimer(float newTime, float duration)
    {
        currentFoodReduce = 0f;
        plusFoodReduceTimer = newTime;
        Invoke(nameof(ResetPlusTime), duration);
    }

    public void ChangePlusReduceTimer(float newTime)
    {
        currentFoodReduce = 0f;
        plusFoodReduceTimer = newTime;
    }
    public void Feed(float v)
    {
        currentFood = Mathf.Min(currentFood + v, GetMaxFood());
        currentFoodReduce = 0f;
    }
    public void Feed(float v, float newTime)
    {
        currentFood = Mathf.Min(currentFood + v, GetMaxFood());
        ChangePlusReduceTimer(newTime);
    }
    public void Feed(float v, float newTime, float duration)
    {
        currentFood = Mathf.Min(currentFood + v, GetMaxFood());
        ChangePlusReduceTimer(newTime, duration);
    }

    private void ResetPlusTime()
    {
        plusFoodReduceTimer = 0f;
    }

    public void ComsumeFood()
    {
        if (currentFood > 0f)
        {
            currentFoodReduce += Time.deltaTime;
            if (currentFoodReduce >= targetFoodReduce + plusFoodReduceTimer)
            {
                currentFoodReduce = targetFoodReduce + plusFoodReduceTimer;
                UseFood();
            }
        }
    }
    public void UseFood()
    {
        currentFood = Mathf.Max(0f, currentFood - Time.deltaTime * reduceFoodRate);
        if (currentFood == 0f)
        {
            currentFoodReduce = 0f;
        }
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
        return RecoverCoin((int)Mathf.Ceil(GetMaxHealth() - GetCurrentHealth()));
    }

    public int RecoverCoin(int v)
    {
        return recoverHealthPricePerUnit * v;
    }
    public float GetNearDamage()
    {
        return near_damage + plusNearDamage;
    }
    public float GetFarDamage()
    {
        return far_damage + plusFarDamage;
    }
    public string GetPetDetail(bool showNextLevel = true)
    {
        StringBuilder builder = new();

        string nextNearDamage = "";
        string nextFarDamage = "";
        string nextFood = "";
        string nextHealth = "";
        string nextTimeBwtAttack = "";
        string nextPatrolDistance = "";

        if (!IsMaxLevel() && showNextLevel)
        {
            int p_nearDamage = 0;
            int p_farDamage = 0;
            int p_food = 0;
            float p_timeBwtAttack = 0;
            int p_health = 0;
            float p_patrolDistance = 0f;
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

            if (p_nearDamage > 0)
            {
                nextNearDamage = " ( +" + p_nearDamage + " cấp kế tiếp )";
            }
            if (p_farDamage > 0)
            {
                nextFarDamage = " ( +" + p_farDamage + " cấp kế tiếp )";
            }
            if (p_food > 0)
            {
                nextFood = " ( +" + p_food + " cấp kế tiếp )";
            }
            if (p_health > 0)
            {
                nextHealth = " ( +" + p_health + " cấp kế tiếp )";
            }
            if (p_timeBwtAttack > 0)
            {
                nextTimeBwtAttack = " ( +" + p_timeBwtAttack + "  cấp kế tiếp )";
            }
            if (p_patrolDistance > 0)
            {
                nextPatrolDistance = " ( +" + p_patrolDistance + " cấp kế tiếp )";
            }
        }


        builder.Append("Sức mạnh đánh gần: ").Append(GetNearDamage()).Append(nextNearDamage).AppendLine();
        builder.Append("Sức mạnh đánh xa: ").Append(GetFarDamage()).Append(nextFarDamage).AppendLine();
        builder.Append("Đói: ").Append(GetMaxFood()).Append(nextFood).AppendLine();
        builder.Append("Máu: ").Append(Mathf.Round(GetCurrentHealth())).Append("/").Append(GetMaxHealth()).Append(nextHealth).AppendLine();
        builder.Append("Thời gian tấn công: ").Append(GetMaxTimeBwtAttack()).Append(nextTimeBwtAttack).AppendLine();
        builder.Append("Khoảng cách tuần tra: ").Append(GetPatrolDistance()).Append(nextPatrolDistance).AppendLine();

        return builder.ToString();
    }
    public string GetLevel()
    {
        return current < levels.Count ? (current + 1).ToString() : "Max";
    }
    public int GetCurrentLevel()
    {
        return current;
    }
    public void RecoverAllFood()
    {
        currentFood = GetMaxFood();
    }

    public bool IsMaxLevel()
    {
        return current >= levels.Count;
    }
    public void Upgrade()
    {
        if (IsMaxLevel())
        {
            LogController.instance.Log("Is Max level now");
            return;
        }
        current += 1;
        currentExe = 0f;
        nextExe *= nextLevelRate;

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
            Pet_Level_Item item = levels[i];
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

        if (plusNearDamage > 0 && hasNearAttack)
        {
            nearAttack = true;
        }

        if (plusFarDamage > 0 && hasFarAttack)
        {
            farAttack = true;
        }

        check.transform.localScale = new(GetPatrolDistance(), GetPatrolDistance(), 1);
    }

    public void Shoot()
    {
        Bullet tempBullet = Instantiate(bullet, bullet_shoot.transform.position, Quaternion.identity);
        tempBullet.BulletInit(transform, bullet_shoot.up, bulletSpeed, GetFarDamage(), bulletDelayDieTime);
    }

    private void OnDrawGizmos()
    {
        if (attackPos != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPos.position, attackRadious);
        }
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