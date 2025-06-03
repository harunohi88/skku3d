using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Boss2AIManager))]
public class Boss_Ferex : AEnemy, IBoss2PatternHandler
{
    // 속성 정의
    public Collider WeaponCollider;
    public Collider WeaponColliderCopied;
    // - 원본 무기
    public GameObject WeaponOriginal;
    // - 복제된 무기
    public GameObject WeaponCopied;
    private int _baseAttackCount = 0;

    public GameObject FireEffectPrefab;
    private GameObject _fireEffectInstance;

    public Vector3 LastIndicatorPosition { get; private set; }

    protected void Start()
    {
        Debug.Log("임시 코드");
        Init(null);
    }

    public void SetIndicatorPosition(Vector3 pos)
    {
        LastIndicatorPosition = pos;
    }

    public override void Init(EnemySpawner spawner)
    {
        base.Init(spawner);
        _stateMachine.ChangeState(new Boss2IdleState());
        BossUIManager.Instance.SetBossUI("Ferex", MaxHealth); ///// HealthBar 추가한 코드
    }

    public override void TakeDamage(Damage damage)
    {
        if (_stateMachine.CurrentState is Boss2DieState) return;
        Health -= damage.Value;
        BossUIManager.Instance.UPdateHealth(Health); ///// HealthBar 추가한 코드

        // 맞았을때 이펙트

        if (Health <= 0)
        {
            ChangeState(new Boss2DieState());
            return;
        }
        Debug.Log("맞음");
    }

    public override void Attack()
    {
        BossEffectManager.Instance.PlayBoss1Particle(0);
        Debug.Log("기본 공격 진입");
        WeaponCollider.enabled = true;
        EnemyRotation.IsFound = false;
    }

    public void OnBaseAttackEnd()
    {
        Debug.Log("기본 공격 해제");
        WeaponCollider.enabled = false;
        _baseAttackCount++;
        if (_baseAttackCount >= 2)
        {
            _baseAttackCount = 0;
            Boss2AIManager.Instance.SetLastFinishedTime(0, Time.time); // 쿨타임 관리
            OnAnimationEnd();
        }
    }

    public void Boss2SpecialAttack_01()
    {
        BossEffectManager.Instance.PlayBoss1Particle(1);
        Debug.Log("특수공격1 진입");
        WeaponCollider.enabled = true;
        EnemyRotation.IsFound = false;
    }

    public void OnBoss2SpecialAttack01End()
    {
        Debug.Log("특수공격1 해제");
        WeaponCollider.enabled = false;
        Boss2AIManager.Instance.SetLastFinishedTime(1, Time.time); // 쿨타임 관리
        OnAnimationEnd();
    }

    public void Boss2SpecialAttack_02()
    {
        BossEffectManager.Instance.PlayBoss1Particle(2);
        BossEffectManager.Instance.PlayBoss1Particle(3);
        Debug.Log("특수공격2 진입");
        WeaponCollider.enabled = true;
        WeaponColliderCopied.enabled = true;
        EnemyRotation.IsFound = false;

        EnemyPatternData _patternData = Boss2AIManager.Instance.GetPatternData(2, 1);
        //if (_patternData != null)
        //{
        //    ClusterInstantiate(
        //        transform.position,
        //        _patternData.MaxCount,
        //        _patternData.Range,
        //        _patternData.Range
        //    );
        //}
        List<Collider> colliderList = Physics.OverlapSphere(transform.position, _patternData.Range, LayerMask).ToList();
        GameObject playerObject = colliderList.Find(x => x.CompareTag("Player"))?.gameObject;

        if (playerObject)
        {
            Vector3 directionToTarget = playerObject.transform.position - transform.position;
            if (Vector3.Dot(transform.position, directionToTarget.normalized) > 0 && Mathf.Abs(Vector3.Dot(transform.right, directionToTarget)) <= _patternData.Width / 2)
            {
                Debug.Log("특수공격 2 데미지 발생");
            }
        }

    }

    public void OnBos22SpecialAttack02End()
    {
        Debug.Log("특수공격2 해제");
        WeaponCollider.enabled = false;
        WeaponColliderCopied.enabled = false;
        Boss2AIManager.Instance.SetLastFinishedTime(2, Time.time); // 쿨타임 관리
        OnAnimationEnd();
    }

    public void Boss2SpecialAttack_03()
    {
        Debug.Log("특수공격3 이펙트 나오는 중");
        BossEffectManager.Instance.PlayBoss1Particle(4);
        BossEffectManager.Instance.PlayBoss1Particle(5);
        BossEffectManager.Instance.PlayBoss1Particle(6);
        BossEffectManager.Instance.PlayBoss1Particle(7);
        BossEffectManager.Instance.PlayBoss1Particle(8);
        Debug.Log("특수공격3 진입");
        WeaponCollider.enabled = true;
        EnemyRotation.IsFound = false;

        if (FireEffectPrefab != null)
        {
            _fireEffectInstance = GameObject.Instantiate(FireEffectPrefab, LastIndicatorPosition, Quaternion.identity);
        }

        EnemyPatternData _patternData = Boss2AIManager.Instance.GetPatternData(3, 1);
        List<Collider> colliderList = Physics.OverlapSphere(transform.position, _patternData.Range, LayerMask).ToList();
        GameObject playerObject = colliderList.Find(x => x.CompareTag("Player"))?.gameObject;

        if (playerObject)
        {
            Vector3 directionToTarget = playerObject.transform.position - transform.position;
            if (Vector3.Dot(transform.forward, directionToTarget.normalized) > 0 && Mathf.Abs(Vector3.Dot(transform.right, directionToTarget)) <= _patternData.Width / 2)
            {
                Debug.Log("특수공격 3 데미지 발생");
            }
        }
    }

    public void OnBoss2SpecialAttack03End()
    {
        Debug.Log("특수공격3 해제");
        WeaponCollider.enabled = false;

        if (_fireEffectInstance != null)
        {
            Destroy(_fireEffectInstance);
            _fireEffectInstance = null;
        }

        Boss2AIManager.Instance.SetLastFinishedTime(3, Time.time); // 쿨타임 관리
        OnAnimationEnd();
    }

    public override void OnAnimationEnd()
    {
        base.OnAnimationEnd();
        ChangeState(new Boss2TraceState());
    }

    //private void ClusterInstantiate(Vector3 center, int count, float radiusX, float radiusY)
    //{
    //    int placed = 0;
    //    var patternData = BossAIManager.Instance.GetPatternData(2);

    //    while (placed < count)
    //    {
    //        float angle = Random.Range(0f, Mathf.PI * 2);
    //        float radius = Mathf.Sqrt(Random.Range(0, 1f));

    //        float x = Mathf.Cos(angle) * radius * radiusX;
    //        float y = Mathf.Sin(angle) * radius * radiusY;

    //        Vector3 spawnPoint = center + new Vector3(x, center.y, y);
    //        Lightning lightning = _lightningPool.Get();
    //        lightning.transform.position = spawnPoint;
    //        lightning.Init(patternData.CastingTime, patternData.Radius, patternData.Duration);
    //        if (lightning.thisPool == null) lightning.thisPool = _lightningPool;

    //        placed++;
    //    }
    //}

    public void NavMeshAgentOff()
    {
        if (Agent != null && Agent.enabled)
        {
            Debug.Log("NavMesh 비활성화");
            Agent.ResetPath();              // 목적지 초기화
            Agent.isStopped = true;         // 이동 중지
            Agent.updatePosition = false;   // 위치 갱신 중지
            Agent.updateRotation = false;   // 회전 갱신 중지
            Agent.enabled = false;          // Agent 자체 비활성화
        }
    }

    public void NavMeshAgentOn()
    {
        if (Agent != null && !Agent.enabled)
        {
            Agent.enabled = true;           // 먼저 다시 켠 후
            Debug.Log("NavMesh 활성화");
        }

        Agent.updatePosition = true;
        Agent.updateRotation = true;
        Agent.isStopped = false;
        Agent.speed = MoveSpeed;            // 기본 속도로 초기화
    }
}
