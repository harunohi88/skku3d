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
        
        SkillInterfaceList[slot]?.Execute();
        //PlayerManager.Instance.PlayerState = EPlayerState.Skill; // 스킬에서 직접 바꿔줌
    }

    public void Cancel()
    {
        CurrentSkill?.Cancel();
        IsTargeting = false;
        
    }
}
