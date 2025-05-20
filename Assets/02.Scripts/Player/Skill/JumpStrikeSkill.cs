using UnityEngine;

public class JumpStrikeSkill : MonoBehaviour, ISkill
{
    [SerializeField] private PlayerSkill _playerSkill;
    [SerializeField] private CooldownManager _cooldownManager;

    public GameObject Indicator;
    LayerMask enemyLayer = LayerMask.GetMask("Enemy");

    string ISkill.SkillName => "JumpStrike";

    public float AttackRange = 7f; // 공격 반경
    public int SkillIndex = 1;
    private float _cooldownTime;

    public bool IsTargeting = false;
    public bool IsAvailable = true;

    public void Execute()
    {
        Debug.Log("Jump Strike Activated");
        if (!IsTargeting)
        {
            _playerSkill.Istargeting = true;
            IsTargeting = true;
            _playerSkill.TargetingSlot = SkillIndex;
            SkillTargeting();
        }
        else
        {
            IsAvailable = false;
            //_animator.SetTrigger("JumpStrike");
            Indicator.SetActive(false);
        }
    }

    private void SkillTargeting()
    {
        Indicator.SetActive(true);
    }

    // 이벤트 시스템에서 호출할 메서드
    public void OnSkillAnimationonHit()
    {
        // 데미지 구현
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, AttackRange, enemyLayer);
        Damage damage = new Damage() { Value = 20, From = PlayerManager.Instance.Player.gameObject };
        foreach (Collider enemy in hitEnemies)
        {
            enemy.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }

    public void OnSkillAnimationonEnd()
    {
        PlayerManager.Instance.PlayerState = EPlayerState.None;
        //쿨다운 매니저에 등록
        _cooldownManager.StartCooldown(_cooldownTime, SetAvailable);
    }

    public void Cancel()
    {
        PlayerManager.Instance.PlayerState = EPlayerState.None;
    }

    public void SetAvailable()
    {
        IsAvailable = true;
    }
}
