using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[RequireComponent(typeof(Boss3AIManager))]
public class Boss_SpiritDemon : AEnemy, ISpecialAttackable
{
    public int ProjectileCount = 3;
    public float ProjectileAngleStep = 30f;

    [Header("Pattern 1")]
    public GameObject KnifePrefab;
    public float KnifeSpawnInterval = 0.1f;

    [Header("Pattern 2")]
    public int VerticalRectCount = 4; // 세로 직사각형 개수
    public float VerticalRectWidth = 2f; // 직사각형의 가로 길이
    public float VerticalRectHeight = 10f; // 직사각형의 세로 길이
    public float VerticalRectSpacing = 2f; // 직사각형 사이의 간격
    public float SphereOuterRadius = 15f;
    public float SphereInnerRadius = 7f;
    public float Pattern2CastingTime = 1f; // 패턴 2의 캐스팅 시간

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
        var patternData = Boss3AIManager.Instance.GetPatternData(1);
        if (patternData != null)
        {
            KnifeInstantiate(
                transform.position,
                patternData.MaxCount,
                patternData.Range,
                patternData.Range
            );
        }
    }

    public void OnSpecialAttack01End()
    {
        Boss3AIManager.Instance.SetLastFinishedTime(1, Time.time);
    }
    
    public void SpecialAttack_02()
    {
        var pattenData = Boss3AIManager.Instance.GetPatternData(2);
        if (pattenData != null)
        {
            Pattern02();
            OnDrawGizmos();
        }
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
        StartCoroutine(KnifeInstantiateCoroutine(center, count, innerRadius, outerRadius));
    }

    private IEnumerator KnifeInstantiateCoroutine(Vector3 center, int count, float innerRadius, float outerRadius)
    {
        int placed = 0;
        var patterData = Boss3AIManager.Instance.GetPatternData(1);
        float spawnInterval = KnifeSpawnInterval; // 각 칼 사이의 소환 간격 (초)
        float indicatorTime = 0.5f; // 인디케이터가 보여질 시간

        while (placed < count)
        {
            // 1. 칼의 시작점과 목표점 계산
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

            // 2. 인디케이터 생성 (칼의 경로를 직사각형으로 표시)
            Vector3 dir = (centerPoint - spawnPoint).normalized;
            float length =  20f;
            Vector3 indicatorCenter = spawnPoint;
            float width = 0.5f; // 칼의 폭(시각적 효과용)
            float height = length;

            // 방향 계산
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            Quaternion lookRot = Quaternion.LookRotation(centerPoint - spawnPoint, Vector3.up);
            // 내부에서 90도(X축) 회전이 이미 적용되므로, Z축이 위로 가는 상태
            // 따라서, Y축 기준으로 원하는 방향에 -90도(X축) 회전을 추가
            Quaternion rotation = lookRot * Quaternion.Euler(90, 0, 0);

            // 인디케이터 생성
            SkillIndicator indicator = BossIndicatorManager.Instance.SetSquareIndicator(
                indicatorCenter,
                width,
                height,
                0, // direction
                0, // innerRange
                indicatorTime,
                0,
                true
            );
            // 인디케이터 방향과 위치 
            // y를 0으로 고정
            indicator.transform.position = new Vector3(indicator.transform.position.x, 1, indicator.transform.position.z);
            indicator.transform.rotation = rotation;

            // 3. 인디케이터가 끝날 때까지 대기
            yield return new WaitForSeconds(indicatorTime);

            // 4. 칼 생성
            Projectile knife = Instantiate(KnifePrefab, spawnPoint, Quaternion.identity).GetComponent<Projectile>();
            Damage damage = new Damage();
            damage.Value = Damage;
            damage.From = this.gameObject;
            knife.Init(damage);
            knife.transform.forward = centerPoint - spawnPoint;

            placed++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void Pattern02()
    {
        StartCoroutine(Pattern02Coroutine());
    }

    private IEnumerator Pattern02Coroutine()
    {
        // 패턴들을 리스트에 추가
        var patterns = new List<System.Action>
        {
            CircleAttack,
            DonutAttack,
            VerticalRectAttack,
            HorizontalRectAttack
        };

        // 리스트를 섞기
        for (int i = patterns.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            var temp = patterns[i];
            patterns[i] = patterns[randomIndex];
            patterns[randomIndex] = temp;
        }

        // 섞인 순서대로 패턴 실행
        foreach (var pattern in patterns)
        {
            pattern.Invoke();
            yield return new WaitForSeconds(Pattern2CastingTime);
        }
    }

    private void CircleAttack()
    {
        float radius = 5f;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                var damageable = hitCollider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    Damage damage = new Damage { Value = Damage, From = gameObject };
                    damageable.TakeDamage(damage);
                }
            }
        }
    }

    private void DonutAttack()
    {
        float innerRadius = SphereInnerRadius;
        float outerRadius = SphereOuterRadius;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, outerRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance > innerRadius)
                {
                    var damageable = hitCollider.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        Damage damage = new Damage { Value = Damage, From = gameObject };
                        damageable.TakeDamage(damage);
                    }
                }
            }
        }
    }

    private void VerticalRectAttack()
    {
        Vector3 startPos = transform.position + transform.right * (-(VerticalRectCount - 1) * (VerticalRectWidth + VerticalRectSpacing) / 2);
        for (int i = 0; i < VerticalRectCount; i++)
        {
            Vector3 rectCenter = startPos + transform.right * (i * (VerticalRectWidth + VerticalRectSpacing));
            Vector3 halfExtents = new Vector3(VerticalRectWidth / 2, 1f, VerticalRectHeight / 2);
            
            Collider[] hitColliders = Physics.OverlapBox(rectCenter, halfExtents, transform.rotation);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    var damageable = hitCollider.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        Damage damage = new Damage { Value = Damage, From = gameObject };
                        damageable.TakeDamage(damage);
                    }
                }
            }
        }
    }

    private void HorizontalRectAttack()
    {
        Vector3 startPos = transform.position + transform.forward * (-(VerticalRectCount - 1) * (VerticalRectWidth + VerticalRectSpacing) / 2);
        for (int i = 0; i < VerticalRectCount; i++)
        {
            Vector3 rectCenter = startPos + transform.forward * (i * (VerticalRectWidth + VerticalRectSpacing));
            Vector3 halfExtents = new Vector3(VerticalRectWidth / 2, 1f, VerticalRectHeight / 2);
            
            Collider[] hitColliders = Physics.OverlapBox(rectCenter, halfExtents, transform.rotation * Quaternion.Euler(0, 90, 0));
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    var damageable = hitCollider.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        Damage damage = new Damage { Value = Damage, From = gameObject };
                        damageable.TakeDamage(damage);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // 원형 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);

        // 도넛 공격 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 3f); // 내부 원
        Gizmos.DrawWireSphere(transform.position, 7f); // 외부 원

        // 세로 직사각형 공격 범위
        Gizmos.color = Color.blue;
        Vector3 startPos = transform.position + transform.right * (-(VerticalRectCount - 1) * (VerticalRectWidth + VerticalRectSpacing) / 2);
        for (int i = 0; i < VerticalRectCount; i++)
        {
            Vector3 rectCenter = startPos + transform.right * (i * (VerticalRectWidth + VerticalRectSpacing));
            Vector3 size = new Vector3(VerticalRectWidth, 0.1f, VerticalRectHeight);
            Gizmos.matrix = Matrix4x4.TRS(rectCenter, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, size);
        }

        // 가로 직사각형 공격 범위
        Gizmos.color = Color.green;
        startPos = transform.position + transform.forward * (-(VerticalRectCount - 1) * (VerticalRectWidth + VerticalRectSpacing) / 2);
        for (int i = 0; i < VerticalRectCount; i++)
        {
            Vector3 rectCenter = startPos + transform.forward * (i * (VerticalRectWidth + VerticalRectSpacing));
            Vector3 size = new Vector3(VerticalRectWidth, 0.1f, VerticalRectHeight);
            Gizmos.matrix = Matrix4x4.TRS(rectCenter, transform.rotation * Quaternion.Euler(0, 90, 0), Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, size);
        }
    }
}
