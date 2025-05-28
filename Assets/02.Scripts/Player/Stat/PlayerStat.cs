using UnityEngine;
using System.Collections.Generic;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private float AttackPower;
    [SerializeField] private float CooldownReduction;
    [SerializeField] private float CriticalHitChance;
    [SerializeField] private float CriticalHitDamage;
    [SerializeField] private float ExperienceGain;
    [SerializeField] private float MaxHealth;
    [SerializeField] private float MaxStamina;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private int ProjectileCountGain;
    
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
            { EStatType.MoveSpeed, new Stat(MoveSpeed) },
            { EStatType.ProjectileCountGain, new Stat(ProjectileCountGain) }
        };
    }
}
