using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class SkillBaseStat
{
    public ESkillStat StatType;
    public float BaseValue;
}

[CreateAssetMenu(fileName = "SkillBaseSO", menuName = "Scriptable Objects/SkillBaseSO")]
public class SkillBaseSO : ScriptableObject
{
    public List<SkillBaseStat> SkillStatList = new List<SkillBaseStat>();
}
