using UnityEngine;

public class TraceState : IState<AEnemy>
{
    public void Enter(AEnemy enemy)
    {
        enemy.SetAnimationTrigger("Run");
        enemy.EnemyRotation.IsFound = true;
    }

    public void Update(AEnemy enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, PlayerManager.Instance.Player.transform.position);
        
        if(distanceToPlayer <= enemy.AttackDistance)
        {
            enemy.ChangeState(new AttackState());
            return;
        }

        enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);
    }

    public void Exit(AEnemy enemy)
    {

    }
}
