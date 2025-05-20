using UnityEngine;

public class BossBaseAttackState : IState<AEnemy>
{
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.SetAnimationTrigger("BaseAttack");
    }

    public void Update(AEnemy enemy)
    {
    }

    public void Exit(AEnemy enemy)
    {
    }
}
