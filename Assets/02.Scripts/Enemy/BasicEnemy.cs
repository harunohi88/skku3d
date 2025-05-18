using UnityEngine;

public class BasicEnemy : AEnemy
{
    protected override void Init()
    {
        base.Init();
        _stateMachine.ChangeState(new IdleState());
    }
}
