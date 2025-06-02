using System.Collections.Generic;
using UnityEngine;

public class RevelationBuffSkill : MonoBehaviour, ISkill
{
    [SerializeField] CooldownManager _cooldownManager;
    public Rune Rune;
    private Animator _animator;
    public string SkillName = "RevelationBuff";

    [Header("버프 특성")]
    private float _attackBonus;
    private float _moveSpeedBonus;

    private float _buffDuration = 10f;
    private float _buffTimer;
    private float _cooldownTime;

    private bool _isBuffActive = false;
    public bool IsAvailable = true;

    private LayerMask _enemyLayer;
    
    public void Initialize()
    {
        _cooldownManager = CooldownManager.Instance;
        _enemyLayer = LayerMask.GetMask("Enemy");
        _animator = PlayerManager.Instance.PlayerSkill.Model.GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (!_isBuffActive) return;

        _buffTimer -= Time.deltaTime;
        if (_buffTimer <= 0f)
        {
            RemoveBuff();
        }
    }

    public void Execute()
    {
        Debug.Log("Revelation Buff Activated");
        if (!IsAvailable) return;

        _buffTimer = _buffDuration;
        _isBuffActive = true; 
        IsAvailable = false;
        //_animator.SetTrigger("Skill3");
    }

    private void ApplyBuff()
    {
        // 플레이어 버프 구현
        // 1. 플레이어 버프 스탯값 조정 - 구조체
        // 공격력 +30%, 이동속도 +20%, 쿨감 +15%, 지속시간 10초
    }
    private void RemoveBuff()
    {
        // 플레이어 버프 삭제. 원래대로.

        PlayerManager.Instance.PlayerState = EPlayerState.None;
        //쿨다운 매니저에 등록
        // _cooldownManager.StartCooldown(_cooldownTime, SetAvailable);
    }

    // 이벤트 시스템에서 호출할 메서드
    public void OnSkillAnimationEffect()
    {
        ApplyBuff();
    }

    public void OnSkillAnimationEnd()
    {
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
        PlayerManager.Instance.PlayerState = EPlayerState.None;
    }

    public void SetAvailable()
    {
        IsAvailable = true;
    }

    public void LevelUp()
    {
        //foreach (KeyValuePair<ESkillStat, Stat> stat in SkillStatDictionary)
        //{
        //    stat.LevelUp();
        //}
    }
}
