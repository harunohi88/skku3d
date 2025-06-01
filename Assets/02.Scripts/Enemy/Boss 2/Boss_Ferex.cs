using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Boss2AIManager))]
public class Boss_Ferex : AEnemy, IBoss2PatternHandler
{
    // 속성 정의
    public Collider WeaponCollider;
    // - 원본 무기
    public GameObject WeaponOriginal;
    // - 복제된 무기
    public GameObject WeaponCopied;
    private int _baseAttackCount = 0;

    protected void Start()
    {
        Debug.Log("임시 코드");
        Init(null);
    }

    public override void Init(EnemySpawner spawner)
    {
        base.Init(spawner);
        _stateMachine.ChangeState(new Boss2IdleState());
        //BossUIManager.Instance.SetBossUI("Ferex", MaxHealth); ///// HealthBar 추가한 코드
    }

    public override void TakeDamage(Damage damage)
    {
        if (_stateMachine.CurrentState is Boss2DieState) return;
        Health -= damage.Value;
        //BossUIManager.Instance.UPdateHealth(Health); ///// HealthBar 추가한 코드

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
        }
    }

    public void Boss2SpecialAttack_01()
    {
        Debug.Log("특수공격1 진입");
        WeaponCollider.enabled = true;
        EnemyRotation.IsFound = false;
    }

    public void OnBoss2SpecialAttack01End()
    {
        Debug.Log("특수공격1 해제");
        WeaponCollider.enabled = false;
        Boss2AIManager.Instance.SetLastFinishedTime(1, Time.time); // 쿨타임 관리
    }

    public void Boss2SpecialAttack_02()
    {
        Debug.Log("특수공격2 진입");
        WeaponCollider.enabled = true;
        EnemyRotation.IsFound = false;

        EnemyPatternData _patternData = Boss2AIManager.Instance.GetPatternData(2, 1);
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
        Boss2AIManager.Instance.SetLastFinishedTime(2, Time.time); // 쿨타임 관리
    }

    public void Boss2SpecialAttack_03()
    {
        Debug.Log("특수공격3 진입");
        WeaponCollider.enabled = true;
        EnemyRotation.IsFound = false;

        EnemyPatternData _patternData = Boss2AIManager.Instance.GetPatternData(3, 1);
        List<Collider> colliderList = Physics.OverlapSphere(transform.position, _patternData.Range, LayerMask).ToList();
        GameObject playerObject = colliderList.Find(x => x.CompareTag("Player"))?.gameObject;

        if (playerObject)
        {
            Vector3 directionToTarget = playerObject.transform.position - transform.position;
            if (Vector3.Dot(transform.position, directionToTarget.normalized) > 0 && Mathf.Abs(Vector3.Dot(transform.right, directionToTarget)) <= _patternData.Width / 2)
            {
                Debug.Log("특수공격 3 데미지 발생");
            }
        }
    }

    public void OnBoss2SpecialAttack03End()
    {
        Debug.Log("특수공격3 해제");
        WeaponCollider.enabled = false;
        Boss2AIManager.Instance.SetLastFinishedTime(3, Time.time); // 쿨타임 관리
    }

    public override void OnAnimationEnd()
    {
        base.OnAnimationEnd();
        ChangeState(new Boss2TraceState());
    }

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
