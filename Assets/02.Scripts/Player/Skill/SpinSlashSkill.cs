using UnityEngine;

public class SpinSlashSkill : MonoBehaviour, ISkill
{
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

    // 이벤트 시스템에서 호출할 메서드
    public void OnSkillAnimationEffect()
    {
        // 데미지 구현
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, AttackRange, _enemyLayer);
        Damage damage = new Damage() { Value = 10, From = PlayerManager.Instance.Player.gameObject };
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    public void OnSkillAnimationEnd()
    {
        _playerManager.PlayerState = EPlayerState.None;
        //쿨다운 매니저에 등록
        _cooldownManager.StartCooldown(_cooldownTime, SetAvailable);
        _playerManager.PlayerSkill.CurrentSkill = null;
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
