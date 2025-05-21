using UnityEngine;

public class JumpStrikeSkill : MonoBehaviour, ISkill
{
    [SerializeField] private PlayerSkill _playerSkill;
    [SerializeField] private CooldownManager _cooldownManager;
    // public GameObject IndicatorPrefab;
    // public TwoCircleIndicator Indicator;
    public string SkillName = "JumpStrike";
    public float AttackRange = 7f; // 공격 반경
    public int SkillIndex = 1;
    public bool IsTargeting = false;
    public bool IsAvailable = true;

    private float _cooldownTime;
    private Animator _animator;
    private LayerMask _enemyLayer;

    public void Initialize()
    {
        // Indicator = Instantiate(IndicatorPrefab).GetComponent<TwoCircleIndicator>();
        // Indicator.gameObject.SetActive(false);
        
        _cooldownManager = CooldownManager.Instance;
        _enemyLayer = LayerMask.GetMask("Enemy");
        _animator = PlayerManager.Instance.PlayerSkill.Model.GetComponent<Animator>();
    }
    
    public void Execute()
    {
        Debug.Log("Jump Strike Activated");
        if (!IsTargeting)
        {
            _playerSkill.IsTargeting = true;
            IsTargeting = true;
            SkillTargeting();
        }
        else
        {
            IsAvailable = false;
            //_animator.SetTrigger("JumpStrike");
            // Indicator.gameObject.SetActive(false);
        }
    }

    private void SkillTargeting()
    {
        // Indicator.gameObject.SetActive(true);
    }

    // 이벤트 시스템에서 호출할 메서드
    public void OnSkillAnimationEffect()
    {
        // 데미지 구현
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, AttackRange, _enemyLayer);
        Damage damage = new Damage() { Value = 20, From = PlayerManager.Instance.Player.gameObject };
        foreach (Collider enemy in hitEnemies)
        {
            enemy.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }

    public void OnSkillAnimationEnd()
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
