using UnityEngine;

public class DamagedState : IState<AEnemy>
{
    private float _time;
    public void Enter(AEnemy enemy)
    {
        enemy.Agent.isStopped = true;
        enemy.Agent.ResetPath();
        enemy.SetAnimationTrigger("Hit");
        _time = 0f;
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if(_time > enemy.DamagedTime)
        {
            enemy.ChangeState(new TraceState());
        }
    }

    public void Exit(AEnemy enemy)
    {
        enemy.Agent.isStopped = false;
    }
}
