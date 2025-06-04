using MoreMountains.Feedbacks;
using UnityEngine;

public class Boss3SpecialAttack04State : IState<AEnemy>
{
    private EnemyPatternData _patternData;
    private float _time = 0f;
    private bool _wasAgentStopped;
    private bool _isStart = false;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);

        _time = 0f;
        _patternData = Boss3AIManager.Instance.GetPatternData(4);

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
                enemy.SetAnimationTrigger("SpecialAttack04");
                _patternData = Boss3AIManager.Instance.GetPatternData(4);

                enemy.EnemyRotation.IsFound = false;
                enemy.Agent.isStopped = true;

                _wasAgentStopped = false;
                _isStart = true;
            }
        }
        else
        {
            _time += Time.deltaTime;
            if (!_wasAgentStopped)
            {
                if (_patternData != null && _time >= 1f)
                {
                    if (enemy is ISpecialAttackable specialAttackable)
                    {
                        specialAttackable.SpecialAttack_04();
                    }
                    _wasAgentStopped = true;
                }
                
            }

            if (_patternData != null && _time >= _patternData.CastingTime)
            {
                if (enemy is ISpecialAttackable specialAttackable)
                {
                    specialAttackable.OnSpecialAttack04End();
                }
                enemy.ChangeState(new Boss3IdleState());
            }
        }  
    }

    public void Exit(AEnemy enemy)
    {
        enemy.Agent.isStopped = false;
        enemy.EnemyRotation.IsFound = true;
    }
}
