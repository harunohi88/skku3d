using UnityEngine;

public class BossBaseAttackState : IState<AEnemy>
{
    private float _originRotationSpeed;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.Agent.ResetPath();
        enemy.SetAnimationTrigger("BaseAttack");
        enemy.Agent.speed = enemy.MoveSpeed + 2;
        enemy.EnemyRotation.IsFound = false;
        _originRotationSpeed = enemy.EnemyRotation.RotationSpeed;
        enemy.EnemyRotation.RotationSpeed = 2f;
    }

    public void Update(AEnemy enemy)
    {
    }

    public void Exit(AEnemy enemy)
    {
        enemy.Agent.speed = enemy.MoveSpeed;
        enemy.EnemyRotation.RotationSpeed = _originRotationSpeed;
    }
}
