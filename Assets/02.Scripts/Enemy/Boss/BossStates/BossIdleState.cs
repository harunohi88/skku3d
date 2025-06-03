using UnityEngine;

public class BossIdleState : IState<AEnemy>
{
    private float _time = 0f;
    public void Enter(AEnemy enemy)
    {
        enemy.SetAnimationTrigger("Idle");
        enemy.Agent.ResetPath();
        enemy.Agent.isStopped = true;
        enemy.EnemyRotation.IsFound = false;
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if (_time >= 1f)
        {
            if (Vector3.Distance(enemy.transform.position, PlayerManager.Instance.Player.transform.position) < enemy.TraceDistance)
            {
                IState<AEnemy> state = BossAIManager.Instance.DecideNextState();
                enemy.ChangeState(state);
            }
        }
    }

    public void Exit(AEnemy enemy)
    {
        enemy.Agent.isStopped = false;
        enemy.EnemyRotation.IsFound = true;
    }
}
