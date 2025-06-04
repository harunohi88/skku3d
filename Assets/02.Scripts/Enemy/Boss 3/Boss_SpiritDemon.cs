using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEditor;

[RequireComponent(typeof(Boss3AIManager))]
public class Boss_SpiritDemon : AEnemy, ISpecialAttackable
{
    public int ProjectileCount = 3;
    public float ProjectileAngleStep = 30f;

    [Header("Pattern 1")]
    public GameObject KnifePrefab;
    public float KnifeSpawnInterval = 0.1f;
    public Vector3 KnifeSpawnYOffset = new Vector3(0, 0.5f, 0);

    [Header("Pattern 2")]
    public int VerticalRectCount = 4; // 세로 직사각형 개수
    public float VerticalRectWidth = 2f; // 직사각형의 가로 길이
    public float VerticalRectHeight = 10f; // 직사각형의 세로 길이
    public float VerticalRectSpacing = 2f; // 직사각형 사이의 간격
    public float SphereOuterRadius = 15f;
    public float SphereInnerRadius = 7f;
    public float Pattern2CastingTime = 1f; // 패턴 2의 캐스팅 시간

    [Header("Pattern 3 (Black Hole)")]
    public GameObject BlackHolePrefab;
    public Vector3 BlackHoleSpawnCenter = Vector3.zero;
    public float BlackHoleSpawnRadius = 10f;
    public float BlackHoleIndicatorDuration = 1f;

    [Header("Pattern 4 (Donut Attack)")]
    public GameObject Pattern4Prefab;
    public GameObject SafeZonePrefab;
    public Vector3 Pattern4SpawnYPosition = new Vector3(0, 15f, 0);
    public float SafeZoneDuration = 8f;
    public Vector3 AttackCircleCenter = Vector3.zero;
    public float AttackCircleOuterRadius = 10f;
    public float SafeCircleRadius = 3f;
    public float AttackCircleIndicatorDuration = 1f;

    private Vector3 _lastSafeCircleCenter;
    private Vector3 _pattern2StartPosition;
    private Quaternion _pattern2StartRotation;
    private Pattern04Effect _pattern4Effect;

    private void Start()
    {
        Init(null);
        // Find and store reference to Pattern04Effect
        _pattern4Effect = Pattern4Prefab.GetComponent<Pattern04Effect>();
        if (_pattern4Effect == null)
        {
            Debug.LogError("Pattern04Effect component not found on Pattern4Prefab!");
        }
    }

    public override void Init(EnemySpawner spawner)
    {
        base.Init(spawner);
        _stateMachine.ChangeState(new Boss3IdleState());
        BossUIManager.Instance.SetBossUI("Tenebrix", MaxHealth);  ///// HealthBar 추가한 코드
    }

    public override void TakeDamage(Damage damage)
    {
        if (_stateMachine.CurrentState is Boss3DieState) return;
        Health -= damage.Value;
        BossUIManager.Instance.UPdateHealth(Health);    ///// HealthBar 추가한 코드

        EnemyFloatingTextManager.Instance.TriggerFeedback(damage.Value, transform.position + Vector3.up * 2f, damage.IsCritical);

        EnemyHitEffect.PlayHitEffect(DamagedTime);

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

    public void OnBaseAttackEnd()
    {
        Boss3AIManager.Instance.SetLastFinishedTime(0, Time.time);
        OnAnimationEnd();
    }

    public void SpecialAttack_01()
    {
        var patternData = Boss3AIManager.Instance.GetPatternData(1);
        if (patternData != null)
        {
            KnifeInstantiate(
                transform.position, // 나중에는 0이 될수도 있고, 지금은 보스 시전 위치
                patternData.MaxCount,
                patternData.InnerRange,
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
        EnemyRotation.IsFound = false;
        var pattenData = Boss3AIManager.Instance.GetPatternData(2);
        if (pattenData != null)
        {
            Pattern02();
        }
    }

    public void OnSpecialAttack02End()
    {
        Boss3AIManager.Instance.SetLastFinishedTime(2, Time.time);
        EnemyRotation.IsFound = true;
    }

    public void SpecialAttack_03()
    {
        // 블랙홀 소환 위치를 랜덤 원 안에서 결정
        Vector2 randomCircle = Random.insideUnitCircle * BlackHoleSpawnRadius;
        Vector3 spawnPos = BlackHoleSpawnCenter + new Vector3(randomCircle.x, 0, randomCircle.y);
        StartCoroutine(ShowBlackHoleIndicatorAndSpawn(spawnPos));
    }

    private IEnumerator ShowBlackHoleIndicatorAndSpawn(Vector3 spawnPos)
    {
        // 인디케이터 생성 (원형)
        float indicatorRadius = 3f; // 필요시 조절
        SkillIndicator indicator = BossIndicatorManager.Instance.SetCircularIndicator(
            spawnPos,
            indicatorRadius * 2,
            indicatorRadius * 2,
            0f,
            360f,
            0f,
            BlackHoleIndicatorDuration,
            0f,
            Color.black,
            true
        );
        yield return new WaitForSeconds(BlackHoleIndicatorDuration);
        Instantiate(BlackHolePrefab, spawnPos, Quaternion.identity);
    }

    public void OnSpecialAttack03End()
    {
        Boss3AIManager.Instance.SetLastFinishedTime(3, Time.time);
    }
    
    public void SpecialAttack_04()
    {
        // 안전지대(작은 원) 중심을 큰 원 범위 내 랜덤으로 결정
        Vector2 randomCircle = Random.insideUnitCircle * (AttackCircleOuterRadius - SafeCircleRadius);
        _lastSafeCircleCenter = AttackCircleCenter + new Vector3(randomCircle.x, 0, randomCircle.y);
        StartCoroutine(ShowAttackAndSafeCircleIndicatorsAndAttack());
    }

    private IEnumerator ShowAttackAndSafeCircleIndicatorsAndAttack()
    {
        // 큰 원(공격 범위) 인디케이터
        SkillIndicator attackIndicator = BossIndicatorManager.Instance.SetCircularIndicator(
            AttackCircleCenter,
            AttackCircleOuterRadius * 2,
            AttackCircleOuterRadius * 2,
            0f,
            360f,
            0f,
            AttackCircleIndicatorDuration,
            0f,
            Color.red,
            true
        );
        // 작은 원(안전지대) 인디케이터
        SkillIndicator safeIndicator = BossIndicatorManager.Instance.SetCircularPriorityIndicator(
            _lastSafeCircleCenter,
            SafeCircleRadius * 2,
            SafeCircleRadius * 2,
            0f,
            360f,
            0f,
            AttackCircleIndicatorDuration,
            0f,
            Color.blue,
            true
        );
        // 안전지대 오브젝트 생성
        GameObject safeZone = Instantiate(SafeZonePrefab, _lastSafeCircleCenter, Quaternion.identity);
        Destroy(safeZone, SafeZoneDuration);

        yield return new WaitForSeconds(AttackCircleIndicatorDuration - 1f);

        // 패턴 4 이펙트 활성화
        if (_pattern4Effect != null)
        {
            _pattern4Effect.gameObject.SetActive(true);
        }

        Collider[] hitColliders = Physics.OverlapSphere(AttackCircleCenter, AttackCircleOuterRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                float distToAttackCenter = Vector3.Distance(AttackCircleCenter, hitCollider.transform.position);
                float distToSafeCenter = Vector3.Distance(_lastSafeCircleCenter, hitCollider.transform.position);
                if (distToAttackCenter <= AttackCircleOuterRadius && distToSafeCenter > SafeCircleRadius)
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

    public void OnSpecialAttack04End()
    {
        Boss3AIManager.Instance.SetLastFinishedTime(4, Time.time);
        // 패턴 4 이펙트 비활성화
        if (_pattern4Effect != null)
        {
            _pattern4Effect.gameObject.SetActive(false);
        }
    }

    public override void OnAnimationEnd()
    {
        base.OnAnimationEnd();
        ChangeState(new Boss3IdleState());
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
        center += KnifeSpawnYOffset;
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
            float length =  60f;
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
                Color.red,
                true
            );
            // 인디케이터 방향과 위치 
            // y를 0으로 고정
            indicator.transform.position = new Vector3(indicator.transform.position.x, 1, indicator.transform.position.z);
            indicator.transform.rotation = rotation;

            // 3. 인디케이터가 끝날 때까지 대기
            //yield return new WaitForSeconds(indicatorTime);

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
        // 패턴 시작 시 보스의 위치와 회전 저장
        _pattern2StartPosition = transform.position;
        _pattern2StartRotation = transform.rotation;
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
        // 원형 인디케이터 생성
        SkillIndicator indicator = BossIndicatorManager.Instance.SetCircularIndicator(
            transform.position,
            20f, // width (지름 = 반지름 * 2)
            20f, // height (지름 = 반지름 * 2)
            0f,  // direction
            360f,  // angleRange
            0f,  // innerRange
            Pattern2CastingTime,
            0f,  // castingPercent
            Color.red,
            true
        );

        // 인디케이터가 끝나면 공격 실행
        StartCoroutine(ExecuteCircleAttackAfterDelay(Pattern2CastingTime));
    }

    private IEnumerator ExecuteCircleAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        BossEffectManager.Instance.PlayBoss1Particle(2);
        
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
        // 도넛형 인디케이터 생성
        SkillIndicator indicator = BossIndicatorManager.Instance.SetCircularIndicator(
            transform.position,
            SphereOuterRadius * 2, // width (외부 지름)
            SphereOuterRadius * 2, // height (외부 지름)
            0f,  // direction
            360f,  // angleRange
            SphereInnerRadius / SphereOuterRadius, // innerRange (내부 원 비율)
            Pattern2CastingTime,
            0f,  // castingPercent
            Color.yellow,
            true
        );

        // 인디케이터가 끝나면 공격 실행
        StartCoroutine(ExecuteDonutAttackAfterDelay(Pattern2CastingTime));
    }

    private IEnumerator ExecuteDonutAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        BossEffectManager.Instance.PlayBoss1Particle(3);
        
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
        // 보스의 로컬 좌표계 기준으로 시작 위치 계산
        Vector3 localStartPos = _pattern2StartRotation * Vector3.right * (-(VerticalRectCount - 1) * (VerticalRectWidth + VerticalRectSpacing) / 2);
        Vector3 startPos = _pattern2StartPosition + localStartPos;

        // 각 직사각형마다 인디케이터 생성
        for (int i = 0; i < VerticalRectCount; i++)
        {
            // 로컬 좌표계 기준으로 직사각형 중심점 계산
            Vector3 localOffset = _pattern2StartRotation * Vector3.right * (i * (VerticalRectWidth + VerticalRectSpacing));
            Vector3 localForwardOffset = _pattern2StartRotation * Vector3.forward * (-VerticalRectHeight / 2);
            Vector3 rectCenter = startPos + localOffset + localForwardOffset;

            SkillIndicator indicator = BossIndicatorManager.Instance.SetSquareIndicator(
                rectCenter,
                VerticalRectWidth,
                VerticalRectHeight * 2,
                0f,
                0f,
                Pattern2CastingTime,
                0f,
                Color.blue,
                true
            );
            
            // 인디케이터의 회전을 보스의 회전을 기준으로 설정
            indicator.transform.rotation = _pattern2StartRotation * Quaternion.Euler(90, 0, 0);
        }

        StartCoroutine(ExecuteVerticalRectAttackAfterDelay(Pattern2CastingTime, startPos));
    }

    private void HorizontalRectAttack()
    {
        // 보스의 로컬 좌표계 기준으로 시작 위치 계산
        Vector3 localStartPos = _pattern2StartRotation * Vector3.forward * (-(VerticalRectCount - 1) * (VerticalRectWidth + VerticalRectSpacing) / 2);
        Vector3 startPos = _pattern2StartPosition + localStartPos;
        
        // 각 직사각형마다 인디케이터 생성
        for (int i = 0; i < VerticalRectCount; i++)
        {
            // 로컬 좌표계 기준으로 직사각형 중심점 계산
            Vector3 localOffset = _pattern2StartRotation * Vector3.forward * (i * (VerticalRectWidth + VerticalRectSpacing));
            Vector3 localRightOffset = _pattern2StartRotation * Vector3.right * (VerticalRectHeight / 2);
            Vector3 localForwardOffset = _pattern2StartRotation * Vector3.forward * (VerticalRectWidth / 2);
            Vector3 rectCenter = startPos + localOffset + localRightOffset + localForwardOffset;
            
            SkillIndicator indicator = BossIndicatorManager.Instance.SetSquareIndicator(
                rectCenter,
                VerticalRectWidth * 2,
                VerticalRectHeight * 2,
                90f,
                0f,
                Pattern2CastingTime,
                0f,
                Color.green,
                true
            );
            
            // 인디케이터의 회전을 보스의 회전을 기준으로 설정
            indicator.transform.rotation = _pattern2StartRotation * Quaternion.Euler(90, -90, 0);
        }

        StartCoroutine(ExecuteHorizontalRectAttackAfterDelay(Pattern2CastingTime, startPos));
    }

    private IEnumerator ExecuteVerticalRectAttackAfterDelay(float delay, Vector3 startPos)
    {
        yield return new WaitForSeconds(delay);
        
        BossEffectManager.Instance.PlayBoss1Particle(0);
        
        for (int i = 0; i < VerticalRectCount; i++)
        {
            // 로컬 좌표계 기준으로 직사각형 중심점 계산
            Vector3 localOffset = _pattern2StartRotation * Vector3.right * (i * (VerticalRectWidth + VerticalRectSpacing));
            Vector3 rectCenter = startPos + localOffset;
            Vector3 halfExtents = new Vector3(VerticalRectWidth / 2, 1f, VerticalRectHeight / 2);

            Collider[] hitColliders = Physics.OverlapBox(rectCenter, halfExtents, _pattern2StartRotation);
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

    private IEnumerator ExecuteHorizontalRectAttackAfterDelay(float delay, Vector3 startPos)
    {
        yield return new WaitForSeconds(delay);

        BossEffectManager.Instance.PlayBoss1Particle(1);
        
        for (int i = 0; i < VerticalRectCount; i++)
        {
            // 로컬 좌표계 기준으로 직사각형 중심점 계산
            Vector3 localOffset = _pattern2StartRotation * Vector3.forward * (i * (VerticalRectWidth + VerticalRectSpacing));
            Vector3 rectCenter = startPos + localOffset;
            Vector3 halfExtents = new Vector3(VerticalRectWidth / 2, 1f, VerticalRectHeight / 2);

            Collider[] hitColliders = Physics.OverlapBox(rectCenter, halfExtents, _pattern2StartRotation * Quaternion.Euler(0, 90, 0));
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
        Gizmos.DrawWireSphere(transform.position, SphereInnerRadius); // 내부 원
        Gizmos.DrawWireSphere(transform.position, SphereOuterRadius); // 외부 원

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
