using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class SkillBaseStat
{
    public ESkillStat StatType;
    public float BaseValue;
    public bool CanLevelUp;
    public float IncreasePerGap;
    public int IncreaseGap;
    
}

[CreateAssetMenu(fileName = "SkillBaseSO", menuName = "Scriptable Objects/SkillBaseSO")]
public class SkillBaseSO : ScriptableObject
{
    public List<SkillBaseStat> SkillStatList = new List<SkillBaseStat>();
}
