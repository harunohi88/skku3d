using UnityEngine;

public class OnBossHitTrigger : ARuneTrigger
{
    public override void Initialize(RuneData data)
    {
    }

    public override bool Trigger(RuneExecuteContext context)
    {
        return(Equals(context.TargetEnemy.Type, EnemyType.Boss));
    }
}

