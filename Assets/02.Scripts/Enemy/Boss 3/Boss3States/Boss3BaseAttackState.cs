using UnityEngine;

public class Boss3BaseAttackState : IState<AEnemy>
{
    private float _time = 0f;
    private bool _isStart = false;
    private EnemyPatternData _patternData;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);

        _time = 0f;
        _patternData = Boss3AIManager.Instance.GetPatternData(0);

        enemy.SetAnimationTrigger("Run");
        enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);
    }

    public void Update(AEnemy enemy)
    {
        if(_isStart == false)
        {
            enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);

            if(enemy.Agent.remainingDistance < enemy.AttackDistance)
            {
                enemy.Agent.ResetPath();
                enemy.SetAnimationTrigger("BaseAttack");
                _isStart = true;
            }
        }
    }

    public void Exit(AEnemy enemy)
    {
        enemy.Agent.speed = enemy.MoveSpeed;
    }
}
