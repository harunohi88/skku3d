using UnityEngine;

public class HealEffect : ARuneEffect
{
    private float _amount;

    public override void Initialize(RuneData data, int tier)
    {
        _amount = data.TierList[tier - 1];
    }

    public override void ApplyEffect(RuneExecuteContext context)
    {
        if (context.Player == null)
        {
            return;
        }

        context.Player.Heal(_amount);
    }
}
