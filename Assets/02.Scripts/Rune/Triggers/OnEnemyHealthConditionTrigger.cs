using UnityEngine;

public class OnEnemyHealthConditionTrigger : ARuneTrigger
{
    private float _bound;

    public override void Initialize(RuneData data)
    {
        _bound = data.HealthPercent;
    }

    public override bool Trigger(RuneExecuteContext context)
    {
        AEnemy target = context.TargetEnemy;

        return (_bound < target.Health / target.MaxHealth);
    }
}
