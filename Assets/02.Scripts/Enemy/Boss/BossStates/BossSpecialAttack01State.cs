using UnityEngine;

public class BossSpecialAttack01State : IState<AEnemy>
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
        
        _time = 0f;

        _patternData = BossAIManager.Instance.GetPatternData(1);
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;

        if (_patternData != null && _time >= _patternData.CastingTime + 0.5f)
        {
            if (enemy is ISpecialAttackable specialAttackable)
            {
                specialAttackable.OnSpecialAttack01End();
            }
            enemy.ChangeState(BossAIManager.Instance.DecideNextState());
        }
    }

    public void Exit(AEnemy enemy)
    {
    }
}
