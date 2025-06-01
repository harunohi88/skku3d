using UnityEngine;

public class Boss3SpecialAttack03State : IState<AEnemy>
{
    private float _time = 0f;
    private EnemyPatternData _patternData;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.SetAnimationTrigger("SpecialAttack03");
        if (enemy is ISpecialAttackable specialAttackable)
        {
            specialAttackable.SpecialAttack_03();
        }
        enemy.EnemyRotation.IsFound = false;
        _patternData = Boss3AIManager.Instance.GetPatternData(3, 0);
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if (_patternData != null && _time >= _patternData.CastingTime + 0.5f)
        {
            if (enemy is ISpecialAttackable specialAttackable)
            {
                specialAttackable.OnSpecialAttack03End();
            }
            enemy.ChangeState(new Boss3TraceState());
        }
    }

    public void Exit(AEnemy enemy)
    {
        enemy.EnemyRotation.IsFound = false;
    }
}
