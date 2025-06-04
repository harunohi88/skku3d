using UnityEngine;

public class Boss3SpecialAttack03State : IState<AEnemy>
{
    private float _time = 0f;
    private EnemyPatternData _patternData;
    private bool _isStart = false;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);

        _time = 0f;
        _patternData = Boss3AIManager.Instance.GetPatternData(3);

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
                enemy.Agent.isStopped = true;
                enemy.SetAnimationTrigger("SpecialAttack03");
                if (enemy is ISpecialAttackable specialAttackable)
                {
                    specialAttackable.SpecialAttack_03();
                }
                enemy.EnemyRotation.IsFound = false;
                _patternData = Boss3AIManager.Instance.GetPatternData(3, 0);
                _isStart = true;
            }
        }
        else
        {
            _time += Time.deltaTime;
            if (_patternData != null && _time >= _patternData.CastingTime + 0.5f)
            {
                if (enemy is ISpecialAttackable specialAttackable)
                {
                    specialAttackable.OnSpecialAttack03End();
                }
                enemy.ChangeState(new Boss3IdleState());
            }
        }
    }

    public void Exit(AEnemy enemy)
    {
        enemy.EnemyRotation.IsFound = false;
    }
}
