using UnityEngine;

public class CriticalDamage : ARuneEquip
{
    public override void Initialize(RuneData data, int tier)
    {
        EquipBuff = new StatBuff(
            buffStatType: EStatType.CriticalDamage,
            buffType: EBuffType.Add,
            buffValue: data.TierList[tier],
            duration: 0,
            isPermanent: true,
            tid: data.TID
            );
    }
}
