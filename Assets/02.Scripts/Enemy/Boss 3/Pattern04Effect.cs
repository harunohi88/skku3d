using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Pattern04Effect : MonoBehaviour
{
    public float projectileDamage = 50f;
    [SerializeField] private Boss_SpiritDemon _boss;
    [SerializeField] private List<Transform> _spawnPositionList;
    [SerializeField] private PatternProJectile _projectilePrefab;

    private List<PatternProJectile> _activeProjectiles = new List<PatternProJectile>();

    private void OnEnable()
    {
        if (_boss == null)
        {
            _boss = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Boss_SpiritDemon>();
        }
        SpawnProjectiles();
    }

    private void OnDisable()
    {
        // Return all active projectiles to the pool
        foreach (var projectile in _activeProjectiles)
        {
            if (projectile != null)
            {
                ProjectilePool.Instance.Return(projectile);
            }
        }
        _activeProjectiles.Clear();
    }

    private void SpawnProjectiles()
    {
        foreach (var spawnPos in _spawnPositionList)
        {
            var projectile = ProjectilePool.Instance.Get();
            projectile.transform.position = spawnPos.position;
            projectile.transform.rotation = spawnPos.rotation;

            Damage damage = new Damage();
            damage.Value = projectileDamage;
            damage.From = _boss.gameObject;
            projectile.Init(damage);

            _activeProjectiles.Add(projectile);
        }
    }
}
