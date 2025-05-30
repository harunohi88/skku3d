using UnityEngine;

public class Boss2SpecialAttack01State : IState<AEnemy>
{
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.SetAnimationTrigger("SpecialAttack01");
        enemy.Agent.isStopped = false;
        enemy.Agent.ResetPath();
        enemy.EnemyRotation.IsFound = true;
    }
    public void Update(AEnemy enemy)
    {
    }

    public void Exit(AEnemy enemy)
    {
    }
}
