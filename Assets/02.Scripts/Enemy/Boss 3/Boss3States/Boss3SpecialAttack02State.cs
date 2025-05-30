using UnityEngine;

public class Boss3SpecialAttack02State : IState<AEnemy>
{
    private float _time = 0f;
    private EnemyPatternData _patternData;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        //enemy.SetAnimationTrigger("SpecialAttack02_1_Idle");
        _time = 0f;
        if (enemy is ISpecialAttackable specialAttackable)
        {
            specialAttackable.SpecialAttack_02();
        }

        _patternData = Boss3AIManager.Instance.GetPatternData(2);
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if (_patternData == null) return;
    }

    public void Exit(AEnemy enemy)
    {
        throw new System.NotImplementedException();
    }
}
