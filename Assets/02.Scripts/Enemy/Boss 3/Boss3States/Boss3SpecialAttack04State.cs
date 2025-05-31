using UnityEngine;

public class Boss3SpecialAttack04State : IState<AEnemy>
{
    private EnemyPatternData _patternData;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        //enemy.SetAnimationTrigger("SpecialAttack04_Idle");
        if (_patternData == null) _patternData = Boss3AIManager.Instance.GetPatternData(4);
        if (enemy is ISpecialAttackable specialAttackable)
        {
            specialAttackable.SpecialAttack_04();
        }
    }

    public void Update(AEnemy enemy)
    {
    }

    public void Exit(AEnemy enemy)
    {
        throw new System.NotImplementedException();
    }
}
