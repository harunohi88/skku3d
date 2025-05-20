using UnityEngine;

public class JumpStrikeSkill : MonoBehaviour, ISkill
{
    public float CoolTime = 6.5f;
    public string SkillName = "JumpStrike";
    public float AttackRange = 7f; // 공격 반경
    LayerMask enemyLayer = LayerMask.GetMask("Enemy");


    public void Activate()
    {
        Debug.Log("Jump Strike Activated");
        // 데미지 구현
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, AttackRange, enemyLayer);
        Damage damage = new Damage() { Value = 20, From = PlayerManager.Instance.Player.gameObject };
        foreach (Collider enemy in hitEnemies)
        {
            enemy.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
        }




        // 애니메이션 구현
        // 스킬 vfx 추가
        // 사운드 추가
    }
}
