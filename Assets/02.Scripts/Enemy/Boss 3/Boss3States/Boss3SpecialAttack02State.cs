using UnityEngine;
using UnityEngine.AI;

public class Boss3SpecialAttack02State : IState<AEnemy>
{
    private float _time = 0f;
    private EnemyPatternData _patternData;
    private bool _wasAgentStopped;

    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);

        // 이동 멈춤
        enemy.Agent.isStopped = true;

        // 회전 멈춤
        enemy.EnemyRotation.IsFound = false;

        
        _time = 0f;


        _wasAgentStopped = false;
        _patternData = Boss3AIManager.Instance.GetPatternData(2);
    }

    public void Update(AEnemy enemy)
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
            enemy.ChangeState(new Boss3TraceState());
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
