using Mono.Cecil.Cil;
using UnityEngine;

public class SpinSlashSkill : MonoBehaviour, ISkill
{
    public Rune Rune;
    public string SkillName = "SpinSlash";
    public float AttackRange = 3f; // 공격 반경
    public bool IsAvailable = true;

    private PlayerManager _playerManager;
    private CooldownManager _cooldownManager;
    private Animator _animator;
    private LayerMask _enemyLayer;
    private float _cooldownTime;
    
    public void Initialize()
    {
        _playerManager = PlayerManager.Instance;
        _cooldownManager = CooldownManager.Instance;
        _enemyLayer = LayerMask.GetMask("Enemy");
        _animator = _playerManager.PlayerSkill.Model.GetComponent<Animator>();
    }
    
    // 즉발기
    public void Execute()
    {
        PlayerManager.Instance.PlayerSkill.CurrentSkill = this;
        Debug.Log("Spin Slash Activated");
        _animator.SetTrigger("Skill1");
        _playerManager.PlayerState = EPlayerState.Skill;
    }

    public RuneExecuteContext SetContext(Damage damage, AEnemy target)
    {
        RuneExecuteContext context = new RuneExecuteContext
        {
            Player = _playerManager.Player,
            Damage = damage,
            Skill = this,
            TargetEnemy = target,
            DistanceToTarget = Vector3.Distance(transform.position, target.transform.position),
            TargetHelthPercentage = target.Health / target.MaxHealth,
            IsKill = target.Health <= damage.Value
        };

        return context;
    }

    public void CheckCritical(ref Damage damage)
    {
        damage.IsCritical = damage.CriticalRate <= Random.Range(0f, 1f);
    }
    
    // 이벤트 시스템에서 호출할 메서드
    public void OnSkillAnimationEffect()
    {
        // 데미지 구현
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, AttackRange, _enemyLayer);
        Damage damage = new Damage() { Value = 100, From = PlayerManager.Instance.Player.gameObject };
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                AEnemy enemyComponent = enemy.gameObject.GetComponent<AEnemy>();
                CheckCritical(ref damage);
                RuneExecuteContext context = SetContext(damage, enemyComponent);
                if (Rune != null && Rune.CheckTrigger(context))
                {
                    Debug.LogWarning("Rune effect applied");
                    Rune.ApplyEffect(context, ref damage);
                }
                damageable.TakeDamage(damage);
                if (Rune != null && Rune.CheckTrigger(context))
                {
                    Debug.LogWarning("Rune effect applied");
                    Rune.ApplyEffect(context, ref damage);
                }
            }
        }
    }

    public void OnSkillAnimationEnd()
    {
        _playerManager.PlayerState = EPlayerState.None;
        _cooldownManager.StartCooldown(_cooldownTime, SetAvailable);
        _playerManager.PlayerSkill.CurrentSkill = null;
    }

    public void EquipRune(Rune rune)
    {
        if (Rune != null)
        {
            UnequipRune();
        }

        // 룬 효과 적용하는 로직 (스탯에 영향을 주는 경우)
        Rune = rune;
    }

    public void UnequipRune()
    {
        if (Rune == null) return;

        // 룬 효과 제거하는 로직 (스탯에 영향을 주는 경우)
        Rune = null;
    }

    public void Cancel()
    {
        _playerManager.PlayerState = EPlayerState.None;
        _cooldownManager.StartCooldown(_cooldownTime, SetAvailable);
        _playerManager.PlayerSkill.CurrentSkill = null;
    }

    private void SetAvailable()
    {
        IsAvailable = true;
    }
}
