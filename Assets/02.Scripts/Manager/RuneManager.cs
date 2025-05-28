using UnityEngine;

public class RuneManager : BehaviourSingleton<RuneManager>
{
    private void Awake()
    {
        RegisterRuneTriggers();
        RegisterRuneEffects(); // 필요 시
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