using System.Net;
using UnityEngine;

public class AttackState : IState<AEnemy>
{
    private float _time;
    public void Enter(AEnemy enemy)
    {
        enemy.SetAnimationTrigger("MoveToAttackDelay");
        enemy.Agent.ResetPath();
        enemy.EnemyRotation.IsFound = true;
        _time = 0f;
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if (_time >= enemy.AttackCooltime)
        {
            enemy.SetAnimationTrigger("AttackDelayToAttack");
            _time = 0f;
        }
    }

    public void Exit(AEnemy enemy)
    {


    }
}
