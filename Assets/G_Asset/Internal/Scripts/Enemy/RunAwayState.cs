using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAwayState : State
{
    float runAwayTimer = 0f;
    float currentRunAwayTimer = 0f;
    NavMeshAgent agent = null;
    float stopDistance = 0f;
    Animator animator = null;
    Vector3 targetPosition = Vector3.zero;
    public override void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        runAwayTimer = enemy.RunAwayTimer();
        currentRunAwayTimer = 0f;
        agent = enemy.GetAgent();
        animator = enemy.GetAnimator();
        stopDistance = enemy.GetStopDistance();
        targetPosition = enemy.GetNewPosition();
        agent.SetDestination(targetPosition);
    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        currentRunAwayTimer += Time.deltaTime;
        if (currentRunAwayTimer >= runAwayTimer)
        {
            enemy.ChangeState(new PatrolState(), "Patrol");
        }
        else
        {
            if (agent.remainingDistance <= stopDistance)
            {
                targetPosition = enemy.GetNewPosition();
                agent.SetDestination(targetPosition);
            }
            float x = targetPosition.x - enemy.transform.position.x;
            float yAxis = x < 0 ? 180f : 0f;
            enemy.transform.rotation = Quaternion.Euler(new(0f, yAxis, 0f));
            animator.SetFloat("Speed", 1f);
        }
    }
}
