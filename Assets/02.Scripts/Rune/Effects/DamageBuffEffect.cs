using UnityEngine;

public class DamageBuffEffect : ARuneEffect
{
    private float _amount;
    private float _tid;
    
    public override void Initialize(RuneData data, int tier)
    {
        _amount = data.TierList[tier - 1];
    }

    public override void ApplyEffect(RuneExecuteContext context, ref Damage damage)
    {
        damage.Value *= (1 + _amount);
    }
}
