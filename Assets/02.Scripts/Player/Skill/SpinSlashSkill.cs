using UnityEngine;

public class SpinSlashSkill : MonoBehaviour, ISkill
{
    [SerializeField] CooldownManager _cooldownManager;

    public GameObject Model;
    LayerMask enemyLayer = LayerMask.GetMask("Enemy");

    string ISkill.SkillName => "SpinSlash";

    public float AttackRange = 3f; // 공격 반경
    private float _cooldownTime;

    public bool IsAvailable = true;

    // 즉발기
    public void Execute()
    {
        Debug.Log("Spin Slash Activated");
        //_animator.SetTrigger("SpinSlash");
        // PlayerState 변경 가능
    }

    // 이벤트 시스템에서 호출할 메서드
    public void OnSkillAnimationonHit()
    {
        // 데미지 구현
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, AttackRange, enemyLayer);
        Damage damage = new Damage() { Value = 10, From = PlayerManager.Instance.Player.gameObject };
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
    }

    private void SetAvailable()
    {
        IsAvailable = true;
    }
}
