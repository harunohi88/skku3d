using System.Collections.Generic;
using UnityEngine;

public static class EnemyTracker
{
    private static readonly HashSet<Transform> _activeEnemies = new();

    public static void Register(Transform enemy)
    {
        _activeEnemies.Add(enemy);
    }

    public static void Unregister(Transform enemy)
    {
        _activeEnemies.Remove(enemy);
    }

    public static IReadOnlyCollection<Transform> GetActiveEnemies()
    {
        return _activeEnemies;
    }
}
