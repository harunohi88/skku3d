using UnityEngine;

public class Boss3IdleState : IState<AEnemy>
{
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
    }

    void IState<AEnemy>.Update(AEnemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, PlayerManager.Instance.Player.transform.position) < enemy.TraceDistance)
        {
            enemy.ChangeState(new Boss3TraceState());
            return;
        }
    }

    
    public void Exit(AEnemy enemy)
    {
    }
}
