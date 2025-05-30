using UnityEngine;

public class OnAfterAttackTrigger : ARuneTrigger
{
    public override void Initialize(RuneData data)
    {
    }

    public override bool Trigger(RuneExecuteContext context)
    {
        return (context.Timing == EffectTimingType.AfterAttack);
    }
}
