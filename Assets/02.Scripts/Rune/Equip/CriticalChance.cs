using UnityEngine;

public class CriticalChance : ARuneEquip
{
    
    public override void Initialize(RuneData data, int tier)
    {
        float criticalChance;

        if (data.CriticalChance != 0)
        {
            criticalChance = data.CriticalChance;
        }
        else
        {
            criticalChance = data.TierList[tier];
        }
        EquipBuff = new StatBuff(
            buffStatType: EStatType.CriticalChance,
            buffType: EBuffType.Add,
            buffValue: criticalChance,
            duration: 0,
            isPermanent: true,
            tid: data.TID
        );
    }
}