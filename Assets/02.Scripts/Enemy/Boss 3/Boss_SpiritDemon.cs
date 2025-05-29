using UnityEngine;

[RequireComponent(typeof(BossAIManager))]
public class Boss_SpiritDemon : AEnemy, ISpecialAttackable
{
    public int ProjectileCount = 3;
    public float ProjectileAngleStep = 30f;

    private void Start()
    {
        Init(null);
    }

    public override void Init(EnemySpawner spawner)
    {
        base.Init(spawner);
        _stateMachine.ChangeState(new Boss3IdleState());
        // BossUIManager.Instance.SetBossUI();  ///// HealthBar 추가한 코드
    }

    public override void TakeDamage(Damage damage)
    {
        if (_stateMachine.CurrentState is Boss3DieState) return;
        Health -= damage.Value;
       //  BossUIManager.Instance.UpdateHealth(Health);    ///// HealthBar 추가한 코드

       if (Health <= 0)
       {
            ChangeState(new Boss3DieState());
            return;
       }

       Debug.Log($"{gameObject.name} {damage.Value} 데미지 입음");
    }

    public override void Attack()
    {
        EnemyRotation.IsFound = false;

        int middleIndex = ProjectileCount / 2;

        for (int i = 0; i < ProjectileCount; i++)
        {
            int offsetFromMiddle = i - middleIndex;

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

    public void SpecialAttack_01()
    {
        throw new System.NotImplementedException();
    }

    public void OnSpecialAttack01End()
    {
        throw new System.NotImplementedException();
    }
    
    public void SpecialAttack_02()
    {
        throw new System.NotImplementedException();
    }

    public void OnSpecialAttack02End()
    {
        throw new System.NotImplementedException();
    }

    public void SpecialAttack_03()
    {
        throw new System.NotImplementedException();
    }

    public void OnSpecialAttack03End()
    {
        throw new System.NotImplementedException();
    }
    
    public void SpecialAttack_04()
    {
        throw new System.NotImplementedException();
    }

    public void OnSpecialAttack04End()
    {
        throw new System.NotImplementedException();
    }

    public override void OnAnimationEnd()
    {
        base.OnAnimationEnd();
        ChangeState(new Boss3TraceState());
    }
}
