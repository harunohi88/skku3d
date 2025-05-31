using UnityEngine;

public class GameManager : BehaviourSingleton<GameManager>
{
    private int _currentStage = 1;

    public int GetCurrentStage() => _currentStage;

    public float GetEnemyBaseDamage(EnemyType type)
    {
        StageData data = DataTable.Instance.GetStageData(10000 + _currentStage - 1);

        switch (type)
        {
            case EnemyType.Basic:
                return data.BasicBaseDamage;
            case EnemyType.Elite:
                return data.EliteBaseDamage;
        }

        return -1;
    }

    public float  GetEnemyBaseHealth(EnemyType type)
    {
        StageData data = DataTable.Instance.GetStageData(10000 + _currentStage - 1);

        switch (type)
        {
            case EnemyType.Basic:
                return data.BasicBaseHealth;
            case EnemyType.Elite:
                return data.EliteBaseHealth;
        }

        return -1;
    }
}
