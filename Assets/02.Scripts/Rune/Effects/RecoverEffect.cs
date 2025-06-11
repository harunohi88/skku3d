using UnityEngine;

public class RecoverEffect : ARuneEffect
{
    private float _amount;

    public override void Initialize(RuneData data, int tier)
    {
        _amount = data.TierList[tier - 1];
    }

    public override void ApplyEffect(RuneExecuteContext context, ref Damage damage)
    {
        if (context.Player == null)
        {
            return;
        }

        context.Player.Heal(_amount);
    }
}
