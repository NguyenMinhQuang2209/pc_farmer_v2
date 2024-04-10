using UnityEngine;
using UnityEngine.AI;

public class AttackState : State
{
    float stopChasingDistance = 0f;
    Health target = null;
    NavMeshAgent agent = null;
    float nearStopAttackDistance = 0f;
    float farAttackDistance = 0f;
    float farTimeBwtAttack = 0f;
    float nearTimeBwtAttack = 0f;
    float currentTimeBwtAttack = 0f;
    Animator animator = null;

    bool nearAttack = false;
    bool farAttack = false;
    public override void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        stopChasingDistance = enemy.GetStopChasingDistance();
        target = enemy.GetTarget();
        agent = enemy.GetAgent();
        nearStopAttackDistance = enemy.GetNearStopAttackDistance();
        animator = enemy.GetAnimator();
        nearTimeBwtAttack = enemy.GetNearTimeBwtAttack();
        farTimeBwtAttack = enemy.GetTimeBwtAttackFarAttack();
        farAttackDistance = enemy.GetFarAttackDistance();
        currentTimeBwtAttack = 0f;
        nearAttack = enemy.UseNearAttack();
        farAttack = enemy.UseFarAttack();
    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        if (!farAttack && !nearAttack)
        {
            enemy.ChangeState(new RunAwayState(), "RunAway");
            return;
        }
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
                if (distance <= nearStopAttackDistance)
                {
                    if (nearAttack)
                    {
                        agent.isStopped = true;
                        agent.SetDestination(enemy.transform.position);
                        agent.isStopped = false;
                        speed = 0f;
                        if (currentTimeBwtAttack >= nearTimeBwtAttack)
                        {
                            currentTimeBwtAttack = 0f;
                            enemy.Attack();
                        }
                    }
                }
                else
                {
                    if (farAttack)
                    {
                        if (distance <= farAttackDistance)
                        {
                            agent.isStopped = true;
                            agent.SetDestination(enemy.transform.position);
                            agent.isStopped = false;
                            speed = 0f;
                            if (currentTimeBwtAttack >= farTimeBwtAttack)
                            {
                                currentTimeBwtAttack = 0f;
                                enemy.FarAttack();
                            }
                        }
                        else
                        {
                            agent.SetDestination(target.transform.position);
                        }
                    }
                    else
                    {
                        agent.SetDestination(target.transform.position);
                    }
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
