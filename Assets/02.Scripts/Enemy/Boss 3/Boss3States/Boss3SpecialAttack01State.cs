using UnityEngine;

public class Boss3SpecialAttack01State : IState<AEnemy>
{
    private float _time = 0f;
    private EnemyPatternData _patternData;
    private bool _wasAgentStopped;
    private bool _isStart = false;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);

        _time = 0f;
        _patternData = Boss3AIManager.Instance.GetPatternData(1);

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
                enemy.EnemyRotation.IsFound = false;
                _time = 0f;
                _wasAgentStopped = false;
                _patternData = Boss3AIManager.Instance.GetPatternData(1);
                _isStart = true;
            }
        }
        else
        {
            _time += Time.deltaTime;

            if (!_wasAgentStopped)
            {
                if (_patternData != null && _time >= 0.5f)
                {

                    enemy.SetAnimationTrigger("SpecialAttack01");
                    if (enemy is ISpecialAttackable specialAttackable)
                    {
                        specialAttackable.SpecialAttack_01();
                    }
                    
                    _wasAgentStopped = true;
                }
            }

            // 변경 될 수도 있음
            // 패턴 데이터가 있고, 시간이 패턴 데이터의 캐스팅 시간 + 0.5초 이상이면
            if (_patternData != null && _time >= _patternData.CastingTime + 0.5f)
            {
                if (enemy is ISpecialAttackable specialAttackable)
                {
                    specialAttackable.OnSpecialAttack01End();
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
