using UnityEngine;

public class Boss2TraceState : IState<AEnemy>
{
    private float _time = 0f;

    public void Enter(AEnemy enemy)
    {
        enemy.SetAnimationTrigger("Walk");
        enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);
    }
    public void Update(AEnemy enemy)
    {
        enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);

        _time += Time.deltaTime;
        if (_time >= 3f)
        {
            IState<AEnemy> state = BossAIManager.Instance.DecideNextState();
            if (state is Boss2TraceState) return;
            else enemy.ChangeState(state);
        }
    }

    public void Exit(AEnemy enemy)
    {
        enemy.Agent.ResetPath();
    }
}
