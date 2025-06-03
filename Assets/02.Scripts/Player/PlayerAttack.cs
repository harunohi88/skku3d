using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AttackSkill : ISkill
{
    public void Initialize()
    {
    }

    public void Execute()
    {
    }

    public void Cancel()
    {
    }

    public void OnSkillAnimationEffect()
    {
    }

    public void OnSkillAnimationEnd()
    {
    }

    public void EquipRune(Rune rune)
    {
    }

    public void UnequipRune()
    {
    }

    public void LevelUp()
    {
    }
}

public class PlayerAttack : MonoBehaviour
{
    public Rune Rune;
    public GameObject Model;
    public List<string> AttackTriggerList;
    public bool IsAttacking;
    public bool InputQueued;
    public SkillBaseSO BaseStat;
    public Dictionary<ESkillStat, Stat> AttackStatDictionary = new Dictionary<ESkillStat, Stat>();

    private ISkill _attackSkill = new AttackSkill();
    private int _dataKey;
    private int _currentAttackIndex;
    private PlayerManager _playerManager;
    private Dictionary<EStatType, Stat> _playerStatDictionary;
    private Animator _animator;
    private LayerMask _enemyLayer;

    private void Awake()
    {
        _animator = Model.GetComponent<Animator>();
    }

    private void Start()
    {
        _playerManager = PlayerManager.Instance;
        _playerStatDictionary = _playerManager.PlayerStat.StatDictionary;
        _enemyLayer = LayerMask.GetMask("Enemy");
        
        foreach (SkillBaseStat baseStat in BaseStat.SkillStatList)
        {
            AttackStatDictionary[baseStat.StatType] = new Stat(baseStat.BaseValue);
        }
    }

    private void LoadData()
    {
        PlayerSkillData attackData = DataTable.Instance.GetPlayerSkillData(_dataKey);
    }
    
    public void Attack()
    {
        PlayerManager.Instance.PlayerState = EPlayerState.Attack;
        IsAttacking = true;
        _animator.SetLayerWeight(1, 1f);
        _playerManager.PlayerRotate.InstantLookAtMouse();
        _animator.SetTrigger(AttackTriggerList[_currentAttackIndex]);
    }

    public void Cancel()
    {
        PlayerManager.Instance.PlayerState = EPlayerState.None;
        _animator.SetLayerWeight(1, 0f);
        InputQueued = false;
        IsAttacking = false;
        _currentAttackIndex = 0;
    }
    
    private List<Collider> GetCollidersInTargetArea()
    {
        float targetRange = AttackStatDictionary[ESkillStat.TargetRange].TotalStat;
        float targetAngle = AttackStatDictionary[ESkillStat.TargetAngle].TotalStat;

        Collider[] colliders = Physics.OverlapSphere(transform.position, targetRange, _enemyLayer);
        List<Collider> filteredColliders = new();

        Vector3 forward = Model.transform.forward;
        Vector3 origin = transform.position;

        foreach (Collider col in colliders)
        {
            Vector3 directionToTarget = (col.transform.position - origin).normalized;
            float angle = Vector3.Angle(forward, directionToTarget);

            if (angle <= targetAngle * 0.5f)
            {
                filteredColliders.Add(col);
            }
        }

        return filteredColliders;
    }

    public Damage SetDamage()
    {
        Damage damage = new Damage();
        damage.Value = _playerStatDictionary[EStatType.AttackPower].TotalStat
                       * AttackStatDictionary[ESkillStat.SkillMultiplier].TotalStat;
        damage.CriticalChance = _playerStatDictionary[EStatType.CriticalChance].TotalStat;
        damage.CriticalDamage = _playerStatDictionary[EStatType.CriticalDamage].TotalStat;
        damage.IsCritical = false;
        damage.From = gameObject;
        return damage;
    }
    
    public void CheckCritical(ref Damage damage)
    {
        damage.IsCritical = damage.CriticalChance >= Random.Range(0f, 1f);
        if (damage.IsCritical)
        {
            damage.Value *= 1f + damage.CriticalDamage;
        }
    }
    
    public RuneExecuteContext SetContext(Damage damage, AEnemy target)
    {
        RuneExecuteContext context = new RuneExecuteContext
        {
            Player = _playerManager.Player,
            Timing = EffectTimingType.BeforeAttack,
            Damage = damage,
            Skill = _attackSkill,
            TargetEnemy = target,
            DistanceToTarget = Vector3.Distance(transform.position, target.transform.position),
            TargetHelthPercentage = target.Health / target.MaxHealth,
            IsKill = target.Health <= damage.Value
        };

        return context;
    }

    private void RuneEffectExecute(RuneExecuteContext context, ref Damage damage)
    {
        if (Rune != null && Rune.CheckTrigger(context))
        {
            Rune.ApplyEffect(context, ref damage);
        }
    }

    public void OnAttackAnimationHit()
    {
        List<Collider> hitEnemies = GetCollidersInTargetArea();
        if (hitEnemies.Count == 0)
        {
            return;
        }

        Damage damage = SetDamage();
        Damage finalDamage;
        RuneExecuteContext context = new RuneExecuteContext();
        
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                finalDamage = damage;
                AEnemy enemyComponent = enemy.gameObject.GetComponent<AEnemy>();
                CheckCritical(ref finalDamage);
                context = SetContext(finalDamage, enemyComponent);
                RuneEffectExecute(context, ref finalDamage);
                context.Timing = EffectTimingType.AfterAttack;
                damageable.TakeDamage(finalDamage);
                RuneEffectExecute(context, ref finalDamage);
            }
        }
        finalDamage = damage;
        context.Timing = EffectTimingType.OncePerAttack;
        RuneEffectExecute(context, ref finalDamage);
    }

    public void OnAttackAnimationEnd()
    {
        if (InputQueued)
        {
            InputQueued = false;
            _currentAttackIndex = (_currentAttackIndex + 1) % AttackTriggerList.Count;
            Attack();
        }
        else
        {
            Cancel();
        }
    }

    public void OnAttackLoopEnd()
    {
        Cancel();
    }
    
    public void EquipRune(Rune rune)
    {
        if (Rune != null)
        {
            UnequipRune();
        }

        Rune = rune;
        Rune.EquipRune();
    }

    public void UnequipRune()
    {
        if (Rune == null) return;

        Rune.UnequipRune();
        Rune = null;
    }
}
