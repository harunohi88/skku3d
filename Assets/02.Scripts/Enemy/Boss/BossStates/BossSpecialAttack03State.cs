using UnityEngine;

public class BossSpecialAttack03State : IState<AEnemy>
{
    private float _time = 0f;
    private int _currentOrder = 0;
    private EnemyPatternData _patternData;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.SetAnimationTrigger("SpecialAttack03_1");
        if (enemy is ISpecialAttackable specialAttackable)
        {
            specialAttackable.SpecialAttack_01();
        }
        _time = 0f;

        _patternData = BossAIManager.Instance.GetPatternData(3, 0);
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if (_patternData == null) return;

        if (_currentOrder == 0)
        {
            if (_time >= _patternData.CastingTime)
            {
                enemy.SetAnimationTrigger("SpecialAttack03_2_Idle");
                enemy.EnemyRotation.IsFound = true;
                _currentOrder++;
                _time = 0f;

                _patternData = BossAIManager.Instance.GetPatternData(3, 1);
            }
        }
        else if (_currentOrder == 1)
        {
            if (_time >= _patternData.CastingTime / 3)
            {
                enemy.SetAnimationTrigger("SpecialAttack03_2");
                enemy.EnemyRotation.IsFound = false;
                _currentOrder++;
                _time = 0f;
            }
        }
        else if (_currentOrder == 2)
        {
            if (_time >= _patternData.CastingTime + 1f)
            {
                enemy.ChangeState(new BossTraceState());
            }
        }
    }

    public void Exit(AEnemy enemy)
    {
    }
}
