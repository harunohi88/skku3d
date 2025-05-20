using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

public class BossAIManager : BehaviourSingleton<BossAIManager>
{
    public AEnemy BossEnemy;

    public List<float> PatternCooltimeList;
    public List<float> lastUsedtimeList;
    public List<float> PatternCastingtimeList;

    [SerializeField] private float _healthThreshold = 0.5f;

    private void Start()
    {
        BossEnemy = GetComponent<AEnemy>();
        lastUsedtimeList = new float[PatternCooltimeList.Count].ToList();
    }

    public IState<AEnemy> DecideNextState()
    {
        float hpRatio = BossEnemy.Health / BossEnemy.MaxHealth;
        List<int> usablePatternList = (hpRatio > _healthThreshold) ? new List<int> { 0, 1, 2 } : new List<int> { 0, 1, 2, 3, 4 };
        List<int> availablePatternList = usablePatternList.Where(x => Time.time - lastUsedtimeList[x] >= PatternCooltimeList[x]).ToList();

        if(availablePatternList.Count > 0)
        {
            int selectedIndex = availablePatternList[Random.Range(0, availablePatternList.Count)];
            lastUsedtimeList[selectedIndex] = Time.time;

            return GetAttackState(selectedIndex);
        }

        return new BossTraceState();
    }

    private IState<AEnemy> GetAttackState(int index)
    {
        switch (index)
        {
            case 0:
                return new BossBaseAttackState();
            case 1:
                return new BossSpecialAttack01State();
            case 2:
                return new BossSpecialAttack02State();
            case 3:
                return new BossSpecialAttack03State();
            case 4:
                return new BossSpecialAttack04State();
            default:
                return new TraceState();
        }
    }
}
