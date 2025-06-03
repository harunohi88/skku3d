using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossAIManager : BehaviourSingleton<BossAIManager>
{
    public AEnemy BossEnemy;
    [SerializeField] private float _healthThreshold = 0.5f;

    [Header("Base Attack Patterns")]
    [SerializeField] private List<EnemyPatternData> _baseAttackPatternList;

    [Header("Special Attack 1 Patterns")]
    [SerializeField] private List<EnemyPatternData> _specialAttack1PatternList;

    [Header("Special Attack 2 Patterns")]
    [SerializeField] private List<EnemyPatternData> _specialAttack2PatternList;

    [Header("Special Attack 3 Patterns")]
    [SerializeField] private List<EnemyPatternData> _specialAttack3PatternList;

    [Header("Special Attack 4 Patterns")]
    [SerializeField] private List<EnemyPatternData> _specialAttack4PatternList;

    public GameObject PortalToNextStage;

    private void Start()
    {
        BossEnemy = GetComponent<AEnemy>();
    }

    public IState<AEnemy> DecideNextState()
    {
        float hpRatio = BossEnemy.Health / BossEnemy.MaxHealth;
        List<int> usablePatternList = (hpRatio > _healthThreshold)
            ? new List<int> { 0, 1, 2 }
            : new List<int> { 0, 1, 2, 3, 4 };

        List<int> availablePatternList = usablePatternList
            .Where(x => IsPatternAvailable(x))
            .ToList();

        if (availablePatternList.Count > 0)
        {
            int selectedIndex = availablePatternList[Random.Range(0, availablePatternList.Count)];

            return GetAttackState(selectedIndex);
        }

        return new BossIdleState();
    }

    private bool IsPatternAvailable(int patternIndex)
    {
        List<EnemyPatternData> patternList = GetPatternList(patternIndex);
        if (patternList == null || patternList.Count == 0) return false;

        EnemyPatternData firstPattern = patternList[0];
        EnemyPatternData lastPattern = patternList[patternList.Count - 1];
        return Time.time - lastPattern.LastFinishedTime >= firstPattern.CoolTime;
    }

    private List<EnemyPatternData> GetPatternList(int patternIndex)
    {
        switch (patternIndex)
        {
            case 0: return _baseAttackPatternList;
            case 1: return _specialAttack1PatternList;
            case 2: return _specialAttack2PatternList;
            case 3: return _specialAttack3PatternList;
            case 4: return _specialAttack4PatternList;
            default: return _baseAttackPatternList;
        }
    }

    public EnemyPatternData GetPatternData(int patternIndex, int subPatternIndex = 0)
    {
        var patterns = GetPatternList(patternIndex);
        if (patterns == null || patterns.Count == 0) return null;

        return patterns[Mathf.Clamp(subPatternIndex, 0, patterns.Count - 1)];


    }

    public void SetLastFinishedTime(int patternIndex, float time)
    {
        var patterns = GetPatternList(patternIndex);
        if (patterns == null) return;

        foreach (var pattern in patterns)
        {
            pattern.LastFinishedTime = time;
        }
    }

    private IState<AEnemy> GetAttackState(int index)
    {
        switch (index)
        {
            case 0: return new BossBaseAttackState();
            case 1: return new BossSpecialAttack01State();
            case 2: return new BossSpecialAttack02State();
            case 3: return new BossSpecialAttack03State();
            case 4: return new BossSpecialAttack04State();
            default: return new BossIdleState();
        }
    }
}
