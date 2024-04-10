using UnityEngine;
using UnityEngine.AI;

public class AttackState : State
{
    float stopChasingDistance = 0f;
    Health target = null;
    NavMeshAgent agent = null;
    float stopAttackDistance = 0f;
    float timeBwtAttack = 0f;
    float currentTimeBwtAttack = 0f;
    Animator animator = null;
    public override void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        stopChasingDistance = enemy.GetStopChasingDistance();
        target = enemy.GetTarget();
        agent = enemy.GetAgent();
        stopAttackDistance = enemy.GetStopAttackDistance();
        animator = enemy.GetAnimator();
        timeBwtAttack = enemy.GetTimeBwtAttack();
        currentTimeBwtAttack = 0f;
    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        bool runAway = enemy.RunAway();
        if (!runAway)
        {
            float distance = Vector2.Distance(enemy.transform.position, target.transform.position);
            currentTimeBwtAttack += Time.deltaTime;
            if (distance > stopChasingDistance)
            {
                agent.isStopped = true;
                enemy.StopChasing();
                agent.isStopped = false;
                enemy.ChangeState(new PatrolState(), "Patrol");
            }
            else
            {
                float speed = 1f;
                float x = target.transform.position.x - enemy.transform.position.x;
                float yAxis = x < 0 ? 180f : 0f;
                enemy.transform.rotation = Quaternion.Euler(new(0f, yAxis, 0f));
                if (distance <= stopAttackDistance)
                {
                    agent.isStopped = true;
                    agent.SetDestination(enemy.transform.position);
                    agent.isStopped = false;
                    speed = 0f;
                    if (currentTimeBwtAttack >= timeBwtAttack)
                    {
                        currentTimeBwtAttack = 0f;
                        enemy.Attack();
                    }
                }
                else
                {
                    agent.SetDestination(target.transform.position);
                }
                animator.SetFloat("Speed", speed);
            }
        }
        else
        {
            agent.isStopped = true;
            enemy.StopChasing();
            agent.isStopped = false;
            enemy.ChangeState(new RunAwayState(), "RunAway");
        }
    }
}
