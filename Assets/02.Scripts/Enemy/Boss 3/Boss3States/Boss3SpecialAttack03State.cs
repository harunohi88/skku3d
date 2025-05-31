using UnityEngine;

public class Boss3SpecialAttack03State : IState<AEnemy>
{
    private float _time = 0f;
    private EnemyPatternData _patternData;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        // enemy.SetAnimationTrigger("SpecialAttack03_1");
        if (enemy is ISpecialAttackable specialAttackable)
        {
            specialAttackable.SpecialAttack_03();
        }

        _patternData = Boss3AIManager.Instance.GetPatternData(3, 0);
    }

    public void Exit(AEnemy enemy)
    {
        throw new System.NotImplementedException();
    }

    public void Update(AEnemy enemy)
    {
    }
}
