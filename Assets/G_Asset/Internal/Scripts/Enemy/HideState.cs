using UnityEngine;

public class HideState : State
{
    public override void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void Exit()
    {

    }

    public override void Perform()
    {

    }
}
