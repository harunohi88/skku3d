using UnityEngine;

public class OnDistanceToEnemyTrigger : ARuneTrigger
{
    private float _bound;
    
    public override void Initialize(RuneData data)
    {
        _bound = data.Distance;
    }

    public override bool Trigger(RuneExecuteContext context)
    {
        AEnemy target = context.TargetEnemy;

        if (target == null)
        {
            return false;
        }

        float distance = Vector3.Distance(context.Player.transform.position, target.transform.position);
        return distance < _bound;
    }
}
