using System.Collections.Generic;
using UnityEngine;

public class Pattern04Effect : MonoBehaviour
{
    List<Projectile> _projectileList;

    public float projectileDamage = 100f;
    [SerializeField] private Boss_SpiritDemon _boss;

    void Start()
    {
        // 모든 자식에서 Projectile 컴포넌트 찾기
        _projectileList = new List<Projectile>(GetComponentsInChildren<Projectile>());
        _boss = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Boss_SpiritDemon>();

        // 각 Projectile에 Init 호출
        foreach (var proj in _projectileList)
        {
            Damage damage = new Damage();
            damage.Value = projectileDamage;
            damage.From = _boss.gameObject; // 혹은 Boss 등 원하는 값으로
            proj.Init(damage);
        }
    }
}
