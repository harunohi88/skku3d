using UnityEngine;

[RequireComponent(typeof(BossAIManager))]
public class Boss_SpiritDemon : AEnemy, ISpecialAttackable
{
    public int ProjectileCount = 3;
    public float ProjectileAngleStep = 30f;

    public GameObject KnifePrefab;

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

    /// <summary>
    /// 칼날 프리펩을 생성하고 방향을 설정한다. 칼날은 Projectile 컴포넌트를 가지고 있다.
    /// </summary>
    /// <param name="center">칼날이 생성될 중심 위치</param>
    /// <param name="count">생성할 칼날의 개수</param>
    /// <param name="innerRadius">칼날의 목표 방향의 반지름</param>
    /// <param name="outerRadius">칼날이 생성될 반지름</param>
    public void KnifeInstantiate(Vector3 center, int count, float innerRadius, float outerRadius)
    {
        int placed = 0;
        var patterData = BossAIManager.Instance.GetPatternData(1);

        while (placed < count)
        {
            // 원 범위 내 랜덤 위치 생성
            // 방향은 center를 바라보게
            float spawnAngle = Random.Range(0f, Mathf.PI * 2);
            float spawnRadius = Mathf.Sqrt(Random.Range(0.5f, 1f));
            

            float spawnX = Mathf.Cos(spawnAngle) * spawnRadius * outerRadius;
            float spawnZ = Mathf.Sin(spawnAngle) * spawnRadius * outerRadius;
            Vector3 spawnPoint = center + new Vector3(spawnX, center.y, spawnZ);

            float centerAngle = Random.Range(0f, Mathf.PI * 2);
            float centerRadius = Mathf.Sqrt(Random.Range(0, 1f));

            float centerX = Mathf.Cos(centerAngle) * centerRadius * innerRadius;
            float centerZ = Mathf.Sin(centerAngle) * centerRadius * innerRadius;
            Vector3 centerPoint = center + new Vector3(centerX, center.y, centerZ);

            Projectile knife = Instantiate(KnifePrefab, spawnPoint, Quaternion.identity).GetComponent<Projectile>();
            Damage damage = new Damage();
            damage.Value = Damage;
            damage.From = this.gameObject;
            knife.Init(damage);
            knife.transform.forward = centerPoint - spawnPoint;

            placed++;
        }
    }


}
