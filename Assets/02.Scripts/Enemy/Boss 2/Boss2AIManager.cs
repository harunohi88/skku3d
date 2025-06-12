using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss2AIManager : BehaviourSingleton<Boss2AIManager>
{
    public AEnemy Boss2Enemy;

    [SerializeField] private float _healthThreshold = 0.5f;

    [Header("기본 공격 패턴")]
    [SerializeField] private List<EnemyPatternData> _boss2BaseAttackPatternList;

    [Header("특수 공격 1 패턴")]
    [SerializeField] private List<EnemyPatternData> _boss2SpecialAttack1PatternList;

    [Header("특수 공격 2 패턴")]
    public List<EnemyPatternData> _boss2SpecialAttack2PatternList;

    [Header("특수 공격 3 패턴")]
    [SerializeField] private List<EnemyPatternData> _boss2SpecialAttack3PatternList;


    public GameObject PortalToNextStage;

    private void Start()
    {
        Boss2Enemy = GetComponent<AEnemy>();
    }

    public IState<AEnemy> DecideNextState()
    {
        float hpRatio = Boss2Enemy.Health / Boss2Enemy.MaxHealth;

        List<int> usablePatternList = (hpRatio > _healthThreshold)
            ? new List<int> { 0, 2, 3 } // 체력비율이 0.5보다 높으면 패턴 0~2만 사용
            : new List<int> { 0, 1, 2, 3 }; // 체력 비율이 0.5보다 낮으면 패턴 0~3까지 사용
        List<int> availablePatternList = usablePatternList // 쿨타임이 지난 패턴만 선택 가능 대상이다.
            .Where(index => IsPatternAvailable(index))
            .ToList();

        if (availablePatternList.Count > 0)
        {
            int selectedIndex = availablePatternList[Random.Range(0, availablePatternList.Count)]; // 사용가능한 패턴 중 랜덤으로 선택한다.
            return GetAttackState(selectedIndex);
        }
        return new Boss2TraceState();
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
            case 0: return _boss2BaseAttackPatternList;
            case 1: return _boss2SpecialAttack1PatternList;
            case 2: return _boss2SpecialAttack2PatternList;
            case 3: return _boss2SpecialAttack3PatternList;
            default: return _boss2BaseAttackPatternList;
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
            case 0: return new Boss2BaseAttackState();
            case 1: return new Boss2SpecialAttack01State();
            case 2: return new Boss2SpecialAttack02State();
            case 3: return new Boss2SpecialAttack03State();
            default: return new Boss2TraceState();
        }
    }
}
