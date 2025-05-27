using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JumpStrikeSkill : MonoBehaviour, ISkill
{
    public GameObject IndicatorPrefab;
    public Rune Rune;
    public float Range;
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
    private Coroutine _moveCoroutine;
    private CharacterController _characterController;

    public void Initialize()
    {
        _characterController = GetComponent<CharacterController>();
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
            Vector3 destination = _indicator.GetTargetPosition();
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
            _playerSkill.Model.transform.forward = (destination - transform.position).normalized;
            _moveCoroutine = StartCoroutine(MoveToTargetCoroutine(destination, MoveDuration));
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

            _characterController.Move(moveDelta); // 장애물 자동 처리

            yield return null;
        }

        // 마지막 위치 정렬 보정 (선택)
        Vector3 finalDelta = destination - transform.position;
        _characterController.Move(finalDelta);
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
        _playerSkill.CurrentSkill = null;
        _cooldownManager.StartCooldown(CooldownTime, SetAvailable); // 쿨다운 등록 스킬 시전시점으로 변경해야됨
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
            OnSkillAnimationEnd();
        }
    }

    public void SetAvailable()
    {
        IsAvailable = true;
    }
}
