using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    float currentWaitTimer = 0f;
    float targetTimer = 0f;
    float stopDistance = 0f;
    NavMeshAgent agent;
    Animator animator;
    Vector3 targetPosition = new();

    public override void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        currentWaitTimer = 0f;
        targetTimer = enemy.GetWaitTime();
        agent = enemy.GetAgent();
        stopDistance = enemy.GetStopDistance();
        animator = enemy.GetAnimator();
        targetPosition = enemy.GetNewPosition();
        agent.SetDestination(targetPosition);
    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        bool sawPlayer = enemy.CanSeeTarget();
        if (!sawPlayer)
        {
            float speed = 1f;
            if (agent.remainingDistance <= stopDistance)
            {
                speed = 0f;
                currentWaitTimer += Time.deltaTime;
                if (currentWaitTimer >= targetTimer)
                {
                    currentWaitTimer = 0f;
                    targetPosition = enemy.GetNewPosition();
                    agent.SetDestination(targetPosition);
                }
            }
            else
            {
                float x = targetPosition.x - enemy.transform.position.x;
                float yAxis = x < 0 ? 180f : 0f;
                enemy.transform.rotation = Quaternion.Euler(new(0f, yAxis, 0f));
            }
            animator.SetFloat("Speed", speed);
        }
        else
        {
            enemy.ChangeState(enemy.SawTargetState());
        }
    }
}
