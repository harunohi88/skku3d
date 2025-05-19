using UnityEngine;

public class IdleState : IState<AEnemy>
{
    public void Enter(AEnemy enemy)
    {

    }

    public void Update(AEnemy enemy)
    {
        if(Vector3.Distance(enemy.transform.position, PlayerManager.Instance.Player.transform.position) < enemy.TraceDistance)
        {
            enemy.ChangeState(new TraceState());
            return;
        }
    }

    public void Exit(AEnemy enemy)
    {

    }
}
