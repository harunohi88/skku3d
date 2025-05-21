using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

public class BossAIManager : BehaviourSingleton<BossAIManager>
{
    public AEnemy BossEnemy;
    [SerializeField] private float _healthThreshold = 0.5f;

    [Header("Base Attack Patterns")]
    [SerializeField] private List<EnemyPatternData> _baseAttackPatterns;

    [Header("Special Attack 1 Patterns")]
    [SerializeField] private List<EnemyPatternData> _specialAttack1Patterns;

    [Header("Special Attack 2 Patterns")]
    [SerializeField] private List<EnemyPatternData> _specialAttack2Patterns;

    [Header("Special Attack 3 Patterns")]
    [SerializeField] private List<EnemyPatternData> _specialAttack3Patterns;

    [Header("Special Attack 4 Patterns")]
    [SerializeField] private List<EnemyPatternData> _specialAttack4Patterns;

    public float Pattern1Radius;
    public float Pattern1Range;
    public float Pattern1LightningLastTime;

    public float Patter2FirstCastingtime;
    public float Pattern2Radius;
    public float Pattern2Range;

    public float Pattern3Range;
    public float Pattern3Angle;

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

        if(availablePatternList.Count > 0)
        {
            int selectedIndex = availablePatternList[Random.Range(0, availablePatternList.Count)];
            return GetAttackState(selectedIndex);
        }

        return new BossTraceState();
    }

    private bool IsPatternAvailable(int patternIndex)
    {
        var patterns = GetPatternList(patternIndex);
        if (patterns == null || patterns.Count == 0) return false;

        return patterns.Any(pattern => 
            Time.time - pattern.LastFinishedTime >= pattern.CoolTime);
    }

    private List<EnemyPatternData> GetPatternList(int patternIndex)
    {
        switch(patternIndex)
        {
            case 0: return _baseAttackPatterns;
            case 1: return _specialAttack1Patterns;
            case 2: return _specialAttack2Patterns;
            case 3: return _specialAttack3Patterns;
            case 4: return _specialAttack4Patterns;
            default: return _baseAttackPatterns;
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
            default: return new TraceState();
        }
    }
}
