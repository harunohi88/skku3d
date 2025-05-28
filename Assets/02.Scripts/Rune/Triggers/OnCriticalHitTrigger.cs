using UnityEngine;

public class OnCriticalHitTrigger : ARuneTrigger
{
    public override void Initialize(RuneData data)
    {
    }
    
    public override bool Trigger(RuneExecuteContext context)
    {
        if (context.TargetEnemy == null || context.TargetEnemy.Health <= 0)
        {
            return false;
        }

        return context.Damage.IsCritical;
    }
}
