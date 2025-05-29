using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossIndicatorManager : BehaviourSingleton<BossIndicatorManager>
{
    private ObjectPool<SkillIndicator> _indicatorPool;
    public SkillIndicator IndicatorPrefab;
    public GameObject PoolParent;

    private void Start()
    {
        _indicatorPool = new ObjectPool<SkillIndicator>(IndicatorPrefab, 20, PoolParent.transform);
    }

    public SkillIndicator SetCircularIndicator(Vector3 position, float width, float height, float direction, float angleRange, float innerRange, float castingTime, float castingPercent, Color color, bool immediateStart = true)
    {
        SkillIndicator indicator = _indicatorPool.Get();
        indicator.CircularInit(width, height, direction, angleRange, innerRange, castingPercent, color, _indicatorPool);
        indicator.SetPosition(position);

        if(immediateStart) indicator.Ready(castingTime);

        return indicator;
    }

    public SkillIndicator SetSquareIndicator(Vector3 position, float width, float height, float direction, float innerRange, float castingTime, float castingPercent, Color color, bool immediateStart = true)
    {
        SkillIndicator indicator = _indicatorPool.Get();
        indicator.SquareInit(width, height, direction, innerRange, castingPercent, color, _indicatorPool);
        indicator.SetPosition(position);

        if (immediateStart) indicator.Ready(castingTime);

        return indicator;
    }
}
