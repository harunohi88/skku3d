using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Boss2AIManager))]
public class Boss_Ferex : AEnemy, IBoss2PatternHandler
{
    // 속성 정의
    public Collider EffectDamageCollider;
    public Collider WeaponCollider;
    public Collider WeaponColliderCopied;
    // - 원본 무기
    public GameObject WeaponOriginal;
    // - 복제된 무기
    public GameObject WeaponCopied;
    private int _baseAttackCount = 0;

    private Coroutine _specialAttack3Coroutine;

    private float _lastWalkSoundTime = 0f;
    private float _walkSoundCooldown = 0.4f;

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

    public void Walk()
    {
        if (Time.time - _lastWalkSoundTime < _walkSoundCooldown) return;
        CameraManager.Instance.CameraShake(0.05f, 2f);
        AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss2Trace, false);
        _lastWalkSoundTime = Time.time;
    }

    public override void Attack()
    {
        CameraManager.Instance.CameraShake(0.07f, 0.8f);
        AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss2Attack);
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
        CameraManager.Instance.CameraShake(1f, 0.8f);
        AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss2Sp1);
        BossEffectManager.Instance.PlayBoss1Particle(1);
        Debug.Log("특수공격1 진입");
        WeaponCollider.enabled = true;
        EnemyRotation.IsFound = false;

        if (EffectDamageCollider != null)

        {
            EffectDamageCollider.enabled = true;
            Debug.Log("EffectDamage 콜라이더 활성화됨");
        }
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

        EnemyPatternData _patternData = Boss2AIManager.Instance.GetPatternData(2, 0);
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
        AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss2Sp3);
        BossEffectManager.Instance.PlayBoss1Particle(4);
        BossEffectManager.Instance.PlayBoss1Particle(5);
        BossEffectManager.Instance.PlayBoss1Particle(6);
        BossEffectManager.Instance.PlayBoss1Particle(7);
        BossEffectManager.Instance.PlayBoss1Particle(8);
        BossEffectManager.Instance.PlayBoss1Particle(9);
        BossEffectManager.Instance.PlayBoss1Particle(10);
        BossEffectManager.Instance.PlayBoss1Particle(12);
        BossEffectManager.Instance.PlayBoss1Particle(13);
        BossEffectManager.Instance.PlayBoss1Particle(14);
        BossEffectManager.Instance.PlayBoss1Particle(15);
        BossEffectManager.Instance.PlayBoss1Particle(16);
        BossEffectManager.Instance.PlayBoss1Particle(17);
        
        Debug.Log("특수공격3 진입");
        WeaponCollider.enabled = true;
        EnemyRotation.IsFound = false;

        float attackDuration = 5f; // 총 지속 시간
        float interval = 1f;       // 1초마다 체크

        if (_specialAttack3Coroutine != null)
            StopCoroutine(_specialAttack3Coroutine);

        _specialAttack3Coroutine = StartCoroutine(CheckSpecialAttack3Damage(attackDuration, interval));
    }

    private IEnumerator CheckSpecialAttack3Damage(float duration, float interval)
    {
        float elapsed = 0f;
        EnemyPatternData _patternData = Boss2AIManager.Instance.GetPatternData(3, 0);
        // EnemyPatternData _patterData2 = Boss2AIManager.Instance.GetPatternData(3, 2);
        Damage damage = new Damage
        {
            Value = _patternData.Damage,
            From = this.gameObject
        };

        while (elapsed < duration)
        {
            elapsed += interval;

            List<Collider> colliderList = Physics.OverlapSphere(transform.position, _patternData.Range, LayerMask).ToList();

            foreach (var col in colliderList)
            {
                if (col.CompareTag("Player"))
                { 
                    PlayerManager.Instance.Player.TakeDamage(damage);
                }
            }

            yield return new WaitForSeconds(interval);
        }
    }

    public void OnBoss2SpecialAttack03End()
    {
        Debug.Log("특수공격3 해제");
        WeaponCollider.enabled = false;

        // 코루틴 정지
        if (_specialAttack3Coroutine != null)
        {
            StopCoroutine(_specialAttack3Coroutine);
            _specialAttack3Coroutine = null;
        }

        Boss2AIManager.Instance.SetLastFinishedTime(3, Time.time); // 쿨타임 관리
        OnAnimationEnd();
    }

    public override void OnAnimationEnd()
    {
        base.OnAnimationEnd();
        ChangeState(new Boss2TraceState());

        StartCoroutine(EnableEffectDamageColliderAfterDelay(1f));
    }


    private IEnumerator EnableEffectDamageColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (EffectDamageCollider != null)
        {
            EffectDamageCollider.enabled = false;
        }
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
