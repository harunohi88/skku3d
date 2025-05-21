using UnityEngine;

public class SpinSlashSkill : MonoBehaviour, ISkill
{
    public PlayerManager PlayerManager;
    public GameObject Model;
    public string SkillName = "SpinSlash";
    public float AttackRange = 3f; // 공격 반경
    public bool IsAvailable = true;

    private CooldownManager _cooldownManager;
    private Animator _animator;
    private LayerMask _enemyLayer;
    private float _cooldownTime;
    
    public void Initialize()
    {
        PlayerManager = PlayerManager.Instance;
        _cooldownManager = CooldownManager.Instance;
        _enemyLayer = LayerMask.GetMask("Enemy");
        _animator = PlayerManager.Instance.PlayerSkill.Model.GetComponent<Animator>();
    }
    
    // 즉발기
    public void Execute()
    {
        PlayerManager.Instance.PlayerSkill.CurrentSkill = this;
        Debug.Log("Spin Slash Activated");
        _animator.SetTrigger("Skill1");
        PlayerManager.Instance.PlayerState = EPlayerState.Skill;
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
        PlayerManager.Instance.PlayerState = EPlayerState.None;
        //쿨다운 매니저에 등록
        _cooldownManager.StartCooldown(_cooldownTime, SetAvailable);
        PlayerManager.Instance.PlayerSkill.CurrentSkill = null;
    }

    public void Cancel()
    {
        PlayerManager.Instance.PlayerState = EPlayerState.None;
        _cooldownManager.StartCooldown(_cooldownTime, SetAvailable);
        PlayerManager.Instance.PlayerSkill.CurrentSkill = null;
    }

    private void SetAvailable()
    {
        IsAvailable = true;
    }
}
