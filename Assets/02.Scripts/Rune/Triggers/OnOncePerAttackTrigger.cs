using UnityEngine;

public class OnOncePerAttackTrigger : ARuneTrigger
{
    public override void Initialize(RuneData data)
    {
    }

    public override bool Trigger(RuneExecuteContext context)
    {
        return (context.Timing == EffectTimingType.OncePerAttack);
    }
}
