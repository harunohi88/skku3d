using UnityEngine;

public class Enemy_Projectile : AEnemy
{
    public int ProjectileCount = 1;
    public float ProjectileAngleStep = 0f;
    public override void Init(EnemySpawner spawner)
    {
        base.Init(spawner);
        _stateMachine.ChangeState(new IdleState());
    }

    public override void Attack()
    {
        EnemyRotation.IsFound = false;

        int middleIndex = ProjectileCount / 2;

        for(int i = 0; i < ProjectileCount; i++)
        {
            int offsetFromMiddle = i - middleIndex;

            // 짝수면 가운데 비우기
            if (ProjectileCount % 2 == 0 && i >= middleIndex) offsetFromMiddle += 1;

            float angle = offsetFromMiddle * ProjectileAngleStep;

            Vector3 dir = Quaternion.AngleAxis(angle, AttackPosition.transform.up) * AttackPosition.transform.forward;

            Projectile projectile = Instantiate(SkillObject, AttackPosition.transform.position, Quaternion.identity).GetComponent<Projectile>();
            Damage damage = new Damage();
            damage.Value = Damage;
            damage.From = this.gameObject;
            projectile.Init(damage);
            projectile.transform.forward = dir;
        }
    }
}