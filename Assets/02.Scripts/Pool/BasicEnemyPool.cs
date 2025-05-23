using UnityEngine;
using System.Collections.Generic;

public class BasicEnemyPool : BehaviourSingleton<BasicEnemyPool>
{
    private ObjectPool<AEnemy> _pool;
    public List<AEnemy> EnemyPrefabList; // 여러 적 프리팹 리스트
    public int PoolSize = 100;

    private void Awake()
    {
        _pool = new ObjectPool<AEnemy>(EnemyPrefabList, PoolSize, transform);
    }

    public AEnemy Get()
    {
        var enemy = _pool.Get();
        return enemy;
    }

    public void Return(AEnemy enemy)
    {
        _pool.Return(enemy);
    }
}
