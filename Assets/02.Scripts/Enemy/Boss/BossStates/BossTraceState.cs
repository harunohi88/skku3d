using UnityEngine;

public class BossTraceState : IState<AEnemy>
{
    // 최소 1초는 기다림
    private float _time = 0f;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.SetAnimationTrigger("Run");
        enemy.EnemyRotation.IsFound = true;
    }

    public void Update(AEnemy enemy)
    {
        enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);

        _time += Time.deltaTime;
        if(_time >= 1f)
        {
            IState<AEnemy> state = BossAIManager.Instance.DecideNextState();
            if (state is BossTraceState) return;
            else enemy.ChangeState(state);
        }
    }

    public void Exit(AEnemy enemy)
    {
    }
}
