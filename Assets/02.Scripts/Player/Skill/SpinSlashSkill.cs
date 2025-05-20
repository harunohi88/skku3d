using UnityEngine;

public class SpinSlashSkill : MonoBehaviour, ISkill
{
    [SerializeField] CooldownManager _cooldownManager;
    private Animator _animator;

    public GameObject Model;
    private LayerMask _enemyLayer;

    public string SkillName = "SpinSlash";

    public float AttackRange = 3f; // 공격 반경
    private float _cooldownTime;

    public bool IsAvailable = true;

    public void Start()
    {
        _cooldownManager = CooldownManager.Instance;
        _enemyLayer = LayerMask.GetMask("Enemy");
        _animator = PlayerManager.Instance.PlayerSkill.Model.GetComponent<Animator>();
    }
    
    // 즉발기
    public void Execute()
    {
        Debug.Log("Spin Slash Activated");
        _animator.SetTrigger("Skill1");
        PlayerManager.Instance.PlayerState = EPlayerState.Skill;
    }

    // 이벤트 시스템에서 호출할 메서드
    public void OnSkillAnimationHit()
    {
        // 데미지 구현
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, AttackRange, _enemyLayer);
        Damage damage = new Damage() { Value = 10, From = PlayerManager.Instance.Player.gameObject };
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
    }

    private void SetAvailable()
    {
        IsAvailable = true;
    }
}
