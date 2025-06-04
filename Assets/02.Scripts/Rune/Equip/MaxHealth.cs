using UnityEngine;

public class MaxHealth : ARuneEquip
{
    public override void Initialize(RuneData data, int tier)
    {
        EquipBuff = new StatBuff(
            buffStatType: EStatType.MaxHealth,
            buffType: EBuffType.Add,
            buffValue: data.TierList[tier],
            duration: 0,
            isPermanent: true,
            tid: data.TID
            );
    }
}
