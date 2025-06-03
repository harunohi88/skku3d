using Mono.Cecil.Cil;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public class SpinSlashSkill : MonoBehaviour, ISkill
{
    public Rune Rune;
    public string SkillName = "SpinSlash";
    public int SkillIndex = 1;
    public int Level = 1;

    public SkillBaseSO SkillBaseStat;
    public Dictionary<ESkillStat, Stat> SkillStatDictionary = new Dictionary<ESkillStat, Stat>();
    
    public bool IsAvailable;

    private PlayerManager _playerManager;
    private Dictionary<EStatType, Stat> _playerStatDictionary;
    private CooldownManager _cooldownManager;
    private Animator _animator;
    private LayerMask _enemyLayer;
    
    public void Initialize()
    {
        _playerManager = PlayerManager.Instance;
        _playerStatDictionary = _playerManager.PlayerStat.StatDictionary;
        _cooldownManager = CooldownManager.Instance;
        _enemyLayer = LayerMask.GetMask("Enemy");
        _animator = _playerManager.PlayerSkill.Model.GetComponent<Animator>();
        IsAvailable = true;

        foreach (SkillBaseStat baseStat in SkillBaseStat.SkillStatList)
        {
            SkillStatDictionary[baseStat.StatType] = new Stat(
                baseStat.BaseValue,
                baseStat.CanLevelUp,
                baseStat.IncreasePerGap,
                baseStat.IncreaseGap);
        }

        UIEventManager.Instance.OnSkillDescriptionChanged?.Invoke(
            SkillIndex,
            Level,
            SkillStatDictionary[ESkillStat.SkillMultiplier].TotalStat);
    }
    
    public void Execute()
    {
        if (!IsAvailable)
        {
            return;
        }

        if (!_playerManager.Player.TryUseStamina(SkillStatDictionary[ESkillStat.SkillCost].TotalStat))
        {
            return;
        }
        
        _cooldownManager.StartCooldown(
            SkillIndex,
            SkillStatDictionary[ESkillStat.SkillCooldown].TotalStat,
            SkillStatDictionary[ESkillStat.SkillCooldown].TotalStat,
            SetAvailable);
        PlayerManager.Instance.PlayerSkill.CurrentSkill = this;
        _animator.SetTrigger("Skill1");
        _playerManager.PlayerState = EPlayerState.Skill;

        // UIEventManager.Instance.OnSkillUse?.Invoke();
    }

    public RuneExecuteContext SetContext(Damage damage, AEnemy target)
    {
        RuneExecuteContext context = new RuneExecuteContext
        {
            Player = _playerManager.Player,
            Timing = EffectTimingType.BeforeAttack,
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
        damage.IsCritical = damage.CriticalChance >= Random.Range(0f, 1f);
        if (damage.IsCritical)
        {
            damage.Value *= 1f + damage.CriticalDamage;
        }
    }
    
    private List<Collider> GetCollidersInTargetArea()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            SkillStatDictionary[ESkillStat.TargetRange].TotalStat,
            _enemyLayer);

        List<Collider> collisionList = colliders.ToList();
        return collisionList;
    }
    
    private void RuneEffectExecute(RuneExecuteContext context, ref Damage damage)
    {
        if (Rune != null && Rune.CheckTrigger(context))
        {
            Rune.ApplyEffect(context, ref damage);
        }
    }

    public Damage SetDamage()
    {
        Damage damage = new Damage();
        damage.Value = _playerStatDictionary[EStatType.AttackPower].TotalStat
                        * SkillStatDictionary[ESkillStat.SkillMultiplier].TotalStat;
        damage.CriticalChance = _playerStatDictionary[EStatType.CriticalChance].TotalStat
                                + SkillStatDictionary[ESkillStat.CriticalChance].TotalStat;
        damage.CriticalDamage = _playerStatDictionary[EStatType.CriticalDamage].TotalStat
                                + SkillStatDictionary[ESkillStat.CriticalDamage].TotalStat;
        damage.IsCritical = false;
        damage.From = _playerManager.Player.gameObject;
        return damage;
    }
    
    public void OnSkillAnimationEffect()
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

    public void OnSkillAnimationEnd()
    {
        _playerManager.PlayerState = EPlayerState.None;
        IsAvailable = false;
        _playerManager.PlayerSkill.CurrentSkill = null;
    }

    public void EquipRune(Rune rune)
    {
        if (Rune != null)
        {
            UnequipRune();
        }
        
        Rune = rune;
        Rune.EquipRune(SkillIndex);
    }

    public void UnequipRune()
    {
        if (Rune == null) return;

        Rune.UnequipRune(SkillIndex);
        Rune = null;
    }

    public void Cancel()
    {
        _playerManager.PlayerState = EPlayerState.None;
        _cooldownManager.StartCooldown(
            SkillIndex,
            SkillStatDictionary[ESkillStat.SkillCooldown].TotalStat,
            SkillStatDictionary[ESkillStat.SkillCooldown].TotalStat,
            SetAvailable);
        _playerManager.PlayerSkill.CurrentSkill = null;
    }

    private void SetAvailable()
    {
        IsAvailable = true;
    }

    public void LevelUp()
    {
        ++Level;
        foreach (KeyValuePair<ESkillStat, Stat> stat in SkillStatDictionary)
        {
            stat.Value.LevelUp();
        }
        UIEventManager.Instance.OnSkillDescriptionChanged?.Invoke(
            SkillIndex,
            Level,
            SkillStatDictionary[ESkillStat.SkillMultiplier].TotalStat);
    }
}
