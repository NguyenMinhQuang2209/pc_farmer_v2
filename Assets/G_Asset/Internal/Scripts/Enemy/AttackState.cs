using UnityEngine;
using UnityEngine.AI;

public class AttackState : State
{
    float stopChasingDistance = 0f;
    Health target = null;
    NavMeshAgent agent = null;
    float stopAttackDistance = 0f;
    Animator animator = null;
    public override void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        stopChasingDistance = enemy.GetStopChasingDistance();
        target = enemy.GetTarget();
        agent = enemy.GetAgent();
        stopAttackDistance = enemy.GetStopAttackDistance();
        animator = enemy.GetAnimator();
    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        float distance = Vector2.Distance(enemy.transform.position, target.transform.position);
        if (distance > stopChasingDistance)
        {
            agent.isStopped = true;
            enemy.StopChasing();
            agent.isStopped = false;
            enemy.ChangeState(new PatrolState());
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
            }
            else
            {
                agent.SetDestination(target.transform.position);
            }
            animator.SetFloat("Speed", speed);
        }
    }
}
