using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RuneManager : BehaviourSingleton<RuneManager>
{
    [Header("동적 룬 투사체 프리펩")]
    [SerializeField] private List<ADynamicRuneObject> _dynamicAttackPrefabList;

    public Dictionary<int, ObjectPool<ADynamicRuneObject>> ProjectilePoolDic = new();

    private void Awake()
    {
        RegisterRuneTriggers();
        RegisterRuneEffects(); // 필요 시

        Global.Instance.OnDataLoaded += InitProjectilePool;
    }

    private void InitProjectilePool()
    {
        int dynamicTID = DataTable.Instance.GetRuneDataList().Find(x => x.RuneType == RuneType.Dynamic).TID;

        for(int i = 0; i < _dynamicAttackPrefabList.Count; i++)
        {
            ProjectilePoolDic.Add(dynamicTID + i, new ObjectPool<ADynamicRuneObject>(_dynamicAttackPrefabList[i], 20, transform));
        }
    }

    private void RegisterRuneTriggers()
    {
        RuneTriggerFactory factory = RuneTriggerFactory.Instance;

        factory.Register("OnCriticalHitTrigger", () => new OnCriticalHitTrigger());
        factory.Register("OnBossHitTrigger", () => new OnBossHitTrigger());
        factory.Register("OnDistanceToEnemyTrigger", () => new OnDistanceToEnemyTrigger());
        factory.Register("OnEnemyHealthConditionTrigger", () => new OnEnemyHealthConditionTrigger());
        factory.Register("OnSkillUseTrigger", () => new OnSkillUseTrigger());
        factory.Register("OnBeforeAttackTrigger", () => new OnBeforeAttackTrigger());
        factory.Register("OnAfterAttackTrigger", () => new OnAfterAttackTrigger());
        factory.Register("OnOncePerAttackTrigger", () => new OnOncePerAttackTrigger());
    }

    private void RegisterRuneEffects()
    {
        RuneEffectFactory factory = RuneEffectFactory.Instance;
        
        factory.Register("DamageBuffEffect", () => new DamageBuffEffect());
        factory.Register("MoveSpeedBuffEffect", () => new MoveSpeedBuffEffect());
        factory.Register("HealEffect", () => new HealEffect());
        factory.Register("SlaughterRuneEffect", () => new SlaughterRuneEffect());
        factory.Register("ArrowRuneEffect", () => new ArrowRuneEffect());
        factory.Register("FlameRuneEffect", () => new FlameRuneEffect());

        factory.Register("FateRuneEffect", () => new FateRuneEffect());
        factory.Register("ElectricRuneEffect", () => new ElectricRuneEffect());
    }

    public void CheckCritical(ref Damage damage)
    {
        damage.IsCritical = damage.CriticalChance >= Random.Range(0f, 1f);
        if (damage.IsCritical)
        {
            damage.Value *= 1f + damage.CriticalDamage;
        }
    }
}