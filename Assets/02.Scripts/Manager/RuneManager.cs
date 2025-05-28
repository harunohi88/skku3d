using UnityEngine;
using System.Collections.Generic;

public class RuneManager : BehaviourSingleton<RuneManager>
{
    [Header("동적 룬 투사체 프리펩")]
    [SerializeField] private List<ADynamicObject> _dynamicAttackPrefab;

    public Dictionary<int, ObjectPool<ADynamicObject>> ProjectilePoolDic = new();

    private void Awake()
    {
        RegisterRuneTriggers();
        RegisterRuneEffects(); // 필요 시

        InitProjectilePool();
    }

    private void InitProjectilePool()
    {
        int dynamicTID = DataTable.Instance.GetRuneDataList().Find(x => x.RuneType == RuneType.Dynamic).TID + 1;

        for(int i = 0; i < DataTable.Instance.GetRuneDataList().Count; i++)
        {
            ProjectilePoolDic.Add(dynamicTID + i, new ObjectPool<ADynamicObject>(_dynamicAttackPrefab, 20, transform));
        }
    }

    private void RegisterRuneTriggers()
    {
        RuneTriggerFactory factory = RuneTriggerFactory.Instance;

        factory.Register("OnCriticalHitTrigger", () => new OnCriticalHitTrigger());
        factory.Register("OnBossHitTrigger", () => new OnBossHitTrigger());
        factory.Register("OnDistanceToEnemyTrigger", () => new OnDistanceToEnemyTrigger());
        factory.Register("OnEnemyHealthConditionTrigger", () => new OnEnemyHealthConditionTrigger());
    }

    private void RegisterRuneEffects()
    {
        RuneEffectFactory factory = RuneEffectFactory.Instance;
        
        factory.Register("DamageBuffEffect", () => new DamageBuffEffect());
        factory.Register("MoveSpeedBuffEffect", () => new MoveSpeedBuffEffect());
        factory.Register("HealEffect", () => new HealEffect());
    }
}