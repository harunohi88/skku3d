using UnityEngine;
using System.Collections.Generic;

public class PlayerStat : MonoBehaviour
{
    public float AttackPower;
    public float CooldownReduction;
    public float CriticalHitChance;
    public float CriticalHitDamage;
    public float ExperienceGain;
    public float MaxHealth;
    public float MaxStamina;
    public float MoveSpeed;
    public Dictionary<EStatType, Stat> StatDictionary;

    private void Awake()
    {
        StatDictionary = new Dictionary<EStatType, Stat>()
        {
            { EStatType.AttackPower, new Stat(AttackPower) }, 
            { EStatType.CooldownReduction, new Stat(CooldownReduction) },
            { EStatType.CriticalHitChance, new Stat(CriticalHitChance) },
            { EStatType.CriticalHitDamage, new Stat(CriticalHitDamage) },
            { EStatType.ExperienceGain, new Stat(ExperienceGain) },
            { EStatType.MaxHealth, new Stat(MaxHealth) },
            { EStatType.MaxStamina, new Stat(MaxStamina) },
            { EStatType.MoveSpeed, new Stat(MoveSpeed) }
        };
    }

}
