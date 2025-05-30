using UnityEngine;

public class BossIdleState : IState<AEnemy>
{
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
    }

    public void Update(AEnemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, PlayerManager.Instance.Player.transform.position) < enemy.TraceDistance)
        {
            enemy.ChangeState(new BossTraceState());
            return;
        }
    }

    public void Exit(AEnemy enemy)
    {

    }
}
