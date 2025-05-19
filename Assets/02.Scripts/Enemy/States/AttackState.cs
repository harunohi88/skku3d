using System.Net;
using UnityEngine;

public class AttackState : IState<AEnemy>
{
    private float _time;
    public void Enter(AEnemy enemy)
    {
        enemy.SetAnimationTrigger("MoveToAttackDelay");
        _time = 0f;
    }

    public void Update(AEnemy enemy)
    {
        if (enemy.IsPlayingAnimation("Attack"))
        {
            enemy.EnemyRotation.IsFound = false;
            return;
        }
        else
        {
            enemy.EnemyRotation.IsFound = true;
        }

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, PlayerManager.Instance.Player.transform.position);

        if (distanceToPlayer >= enemy.AttackOutDistance)
        {
            enemy.ChangeState(new TraceState());
            return;
        }

        _time += Time.deltaTime;
        if(_time >= enemy.AttackCooltime)
        {
            enemy.SetAnimationTrigger("AttackDelayToAttack");
            _time = 0f;
        }
    }

    public void Exit(AEnemy enemy)
    {


    }
}
