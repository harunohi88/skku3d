using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public List<GameObject> SkillList;
    public List<ISkill> SkillInterfaceList = new List<ISkill>();
    public ISkill CurrentSkill;
    public GameObject Model;
    public bool IsTargeting = false;

    public void Start()
    {
        foreach (GameObject obj in SkillList)
        {
            ISkill skill = obj.GetComponent<ISkill>();
            skill.Initialize();
            SkillInterfaceList.Add(skill);
        }
    }

    public void UseSkill(int slot)
    {
        if (slot < 0 || slot >= SkillList.Count) return;
        
        
        // 스킬 타겟팅중 발생하는 입력을 처리할 로직이 필요함
        SkillInterfaceList[slot]?.Execute();
    }

    public void Cancel()
    {
        CurrentSkill?.Cancel();
        IsTargeting = false;
        
    }
}
