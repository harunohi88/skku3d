using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JumpStrikeSkill : MonoBehaviour, ISkill
{
    public Rune Rune;
    public GameObject IndicatorPrefab;
    public SkillBaseSO SkillBaseStat;
    public Dictionary<ESkillStat, Stat> SkillStatDictionary = new Dictionary<ESkillStat, Stat>();
    public bool IsTargeting = false;
    public bool IsAvailable = true;

    private PlayerManager _playerManager;
    private PlayerSkill _playerSkill;
    private Dictionary<EStatType, Stat> _playerStatDictionary;
    private CooldownManager _cooldownManager;
    private TwoCircleIndicator _indicator;
    private Animator _animator;
    private LayerMask _enemyLayer;
    private Coroutine _moveCoroutine;
    private CharacterController _characterController;

    public void Initialize()
    {
        _playerManager = PlayerManager.Instance;
        _playerSkill = _playerManager.PlayerSkill;
        _playerStatDictionary = _playerManager.PlayerStat.StatDictionary;
        _cooldownManager = CooldownManager.Instance;
        _characterController = GetComponent<CharacterController>();
        _enemyLayer = LayerMask.GetMask("Enemy");
        _animator = PlayerManager.Instance.PlayerSkill.Model.GetComponent<Animator>();

        foreach (SkillBaseStat baseStat in SkillBaseStat.SkillStatList)
        {
            SkillStatDictionary[baseStat.StatType] = new Stat(baseStat.BaseValue);
        }
        _indicator = Instantiate(IndicatorPrefab).GetComponent<TwoCircleIndicator>();
        _indicator.SetAreaOfEffects(
            SkillStatDictionary[ESkillStat.SkillRange].TotalStat,
            SkillStatDictionary[ESkillStat.TargetRange].TotalStat);
        _indicator.gameObject.SetActive(false);
    }
    
    public void Execute()
    {
        if (!IsTargeting)
        {
            PlayerManager.Instance.PlayerState = EPlayerState.Targeting;
            _playerSkill.CurrentSkill = this;
            _playerSkill.IsTargeting = true;
            IsTargeting = true;
            _indicator.gameObject.SetActive(true);

            UIEventManager.Instance.OnSkillUse?.Invoke();
        }
        else
        {
            if (_playerManager.Player.TryUseStamina(SkillStatDictionary[ESkillStat.SkillCost].TotalStat) == false)
            {
                return;
            }
            _indicator.gameObject.SetActive(false);
            _playerSkill.IsTargeting = false;
            PlayerManager.Instance.PlayerState = EPlayerState.Skill;
            IsTargeting = false;
            IsAvailable = false;
            Vector3 destination = _indicator.GetTargetPosition();
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
            _playerSkill.Model.transform.forward = (destination - transform.position).normalized;
            _moveCoroutine = StartCoroutine(MoveToTargetCoroutine(destination, SkillStatDictionary[ESkillStat.MoveDuration].TotalStat));
            _animator.SetTrigger("Skill2");
        }
    }

    private IEnumerator MoveToTargetCoroutine(Vector3 destination, float duration)
    {
        Vector3 start = transform.position;
        Vector3 direction = (destination - start).normalized;
        float distance = Vector3.Distance(start, destination);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float delta = Time.deltaTime;
            elapsed += delta;

            float moveSpeed = distance / duration;
            Vector3 moveDelta = direction * moveSpeed * delta;

            _characterController.Move(moveDelta);

            yield return null;
        }

        _moveCoroutine = null;
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
            Skill = this,
            TargetEnemy = target,
            DistanceToTarget = Vector3.Distance(transform.position, target.transform.position),
            TargetHelthPercentage = target.Health / target.MaxHealth,
            IsKill = target.Health <= damage.Value
        };

        return context;
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
                damageable.TakeDamage(damage);
                RuneEffectExecute(context, ref finalDamage);
            }
        }

        finalDamage = damage;
        context.Timing = EffectTimingType.OncePerAttack;
        RuneEffectExecute(context, ref finalDamage);
    }

    public void OnSkillAnimationEnd()
    {
        Debug.Log("Jump Strike End");
        PlayerManager.Instance.PlayerState = EPlayerState.None;
        _playerSkill.CurrentSkill = null;
        _cooldownManager.StartCooldown(
            SkillStatDictionary[ESkillStat.SkillCooldown].TotalStat,
            SetAvailable); // 쿨다운 등록 스킬 시전시점으로 변경해야됨
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
        if (IsTargeting)
        {
            IsTargeting = false;
            _playerSkill.IsTargeting = false;
            _playerSkill.CurrentSkill = null;
            _indicator.gameObject.SetActive(false);
            PlayerManager.Instance.PlayerState = EPlayerState.None;
        }
        else
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }
            OnSkillAnimationEnd();
        }
    }

    public void SetAvailable()
    {
        IsAvailable = true;
    }

    public void LevelUp()
    {
        foreach (KeyValuePair<ESkillStat, Stat> stat in SkillStatDictionary)
        {
            stat.Value.LevelUp();
        }
    }
}
