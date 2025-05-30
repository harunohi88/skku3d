using UnityEngine;

public class EnemyDifficultyMultiplier
{
    public float EnemyCountMultiplier;
    public float EnemyHealthMultiplier;
    public float EnemyDamageMultiplier;

    public EnemyDifficultyMultiplier(float countMultiplier, float healthMultiplier, float damageMultiplier)
    {
        EnemyCountMultiplier = countMultiplier;
        EnemyHealthMultiplier = healthMultiplier;
        EnemyDamageMultiplier = damageMultiplier;
    }
}
