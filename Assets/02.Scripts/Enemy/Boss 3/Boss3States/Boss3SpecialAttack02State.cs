using UnityEngine;
using UnityEngine.AI;

public class Boss3SpecialAttack02State : IState<AEnemy>
{
    private float _time = 0f;
    private EnemyPatternData _patternData;
    private bool _wasAgentStopped;
    private bool _isStart = false;

    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);

        _time = 0f;
        _patternData = Boss3AIManager.Instance.GetPatternData(2);

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
                // 이동 멈춤
                enemy.Agent.isStopped = true;

                // 회전 멈춤
                enemy.EnemyRotation.IsFound = false;
                _time = 0f;
                _wasAgentStopped = false;
                _patternData = Boss3AIManager.Instance.GetPatternData(2);
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

                    enemy.SetAnimationTrigger("SpecialAttack02");
                    if (enemy is ISpecialAttackable specialAttackable)
                    {
                        specialAttackable.SpecialAttack_02();
                    }
                    _wasAgentStopped = true;
                }
            }
            
            if (_patternData != null && _time >= _patternData.CastingTime)
            {
                if (enemy is ISpecialAttackable specialAttackable)
                {
                    specialAttackable.OnSpecialAttack02End();
                }
                enemy.ChangeState(new Boss3IdleState());
            }
        }
    }

    public void Exit(AEnemy enemy)
    {
        // 이동 재개
        enemy.Agent.isStopped = false;

        // 회전 재개
        enemy.EnemyRotation.IsFound = true;
    }
}
