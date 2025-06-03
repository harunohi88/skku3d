using UnityEngine;

public class Boss3BaseAttackState : IState<AEnemy>
{
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.Agent.ResetPath();
        enemy.SetAnimationTrigger("BaseAttack");
        //enemy.EnemyRotation.IsFound = false;
    }

    public void Update(AEnemy enemy)
    {
    }

    public void Exit(AEnemy enemy)
    {
    }
}
