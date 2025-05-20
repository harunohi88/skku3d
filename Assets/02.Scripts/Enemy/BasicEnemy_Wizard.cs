using UnityEngine;

public class BasicEnemy_Wizard : AEnemy
{
    public override void Init()
    {
        base.Init();
        _stateMachine.ChangeState(new IdleState());
    }

    public override void Attack()
    {
        EnemyRotation.IsFound = false;
        GameObject projectile = Instantiate(SkillObject, AttackPosition.transform.position, Quaternion.identity);
        projectile.transform.forward = AttackPosition.transform.forward;
    }
}
