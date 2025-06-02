using System;
using UnityEngine;

public class CooldownEntry
{
    public int SkillIndex;
    public float MaxCooldownTime;
    public float CooldownTime;
    public Action OnCooltimeEnd;
}
