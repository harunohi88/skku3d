using System.Collections.Generic;
using UnityEngine;

public class BossIndicatorManager : BehaviourSingleton<BossIndicatorManager>
{
    private ObjectPool<SkillIndicator> _circularIndicatorPool;
    public List<SkillIndicator> CircularIndicatorPrefabList;
    public GameObject PoolParent;

    private void Start()
    {
        _circularIndicatorPool = new ObjectPool<SkillIndicator>(CircularIndicatorPrefabList, 20, PoolParent.transform);
    }

    public SkillIndicator SetIndicator(Vector3 position, float width, float height, float direction, float angleRange, float innerRange, float castingTime, float castingPercent, bool immediateStart = true)
    {
        SkillIndicator indicator = _circularIndicatorPool.Get();
        indicator.Init(width, height, direction, angleRange, innerRange, castingPercent, _circularIndicatorPool);
        indicator.transform.position = position;

        if(immediateStart) indicator.Ready(castingTime);

        return indicator;
    }
}
