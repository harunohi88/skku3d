using UnityEngine;

public class Boss3SpecialAttack01State : IState<AEnemy>
{
    private float _time = 0f;
    private EnemyPatternData _patternData;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.SetAnimationTrigger("SpecialAttack01");
        if (enemy is ISpecialAttackable specialAttackable)
        {
            specialAttackable.SpecialAttack_01();
        }
        enemy.EnemyRotation.IsFound = false;
        _patternData = Boss3AIManager.Instance.GetPatternData(1);
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        // 변경 될 수도 있음
        // 패턴 데이터가 있고, 시간이 패턴 데이터의 캐스팅 시간 + 0.5초 이상이면
        if (_patternData != null && _time >= _patternData.CastingTime + 0.5f)
        {
            if (enemy is ISpecialAttackable specialAttackable)
            {
                specialAttackable.OnSpecialAttack01End();
            }
            enemy.ChangeState(new Boss3TraceState());
        }
    }

    public void Exit(AEnemy enemy)
    {
        enemy.EnemyRotation.IsFound = true;
    }
}
