using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JumpStrikeSkill : MonoBehaviour, ISkill
{
    public GameObject IndicatorPrefab;
    public float Range; // 공격 반경
    public float TargetAreaRadius;
    public float SkillDamage;
    public bool IsTargeting = false;
    public bool IsAvailable = true;
    public float MoveDuration;
    public float CooldownTime;
    
    private PlayerSkill _playerSkill;
    private CooldownManager _cooldownManager;
    private TwoCircleIndicator _indicator;
    private Animator _animator;
    private LayerMask _enemyLayer;

    public void Initialize()
    {
        _playerSkill = PlayerManager.Instance.PlayerSkill;
        _cooldownManager = CooldownManager.Instance;
        _enemyLayer = LayerMask.GetMask("Enemy");
        _animator = PlayerManager.Instance.PlayerSkill.Model.GetComponent<Animator>();

        _indicator = Instantiate(IndicatorPrefab).GetComponent<TwoCircleIndicator>();
        _indicator.SetAreaOfEffects(Range, TargetAreaRadius);
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
        }
        else
        {
            _indicator.gameObject.SetActive(false);
            _playerSkill.IsTargeting = false;
            PlayerManager.Instance.PlayerState = EPlayerState.Skill;
            IsTargeting = false;
            IsAvailable = false;
            _animator.SetTrigger("Skill2");
        }
    }

    private List<Collider> GetCollidersInTargetArea()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, TargetAreaRadius);

        List<Collider> collisionList = colliders.ToList();
        return collisionList;
    }
    
    public void OnSkillAnimationEffect()
    {
        Debug.Log("Jump Strike Activated");
        List<Collider> collisionList = GetCollidersInTargetArea();
        Damage damage = new() { Value = (int)SkillDamage, From = PlayerManager.Instance.Player.gameObject };
        foreach (Collider obj in collisionList)
        {
            if (obj.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    public void OnSkillAnimationEnd()
    {
        Debug.Log("Jump Strike End");
        PlayerManager.Instance.PlayerState = EPlayerState.None;
        _cooldownManager.StartCooldown(CooldownTime, SetAvailable);
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
    }

    public void SetAvailable()
    {
        IsAvailable = true;
    }
}
