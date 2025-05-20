using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public List<GameObject> SkillList;
    public GameObject Model;
    public bool Istargeting = false;
    public int TargetingSlot;
    
    public List<ISkill> SkillInterfaceList;

    public void Start()
    {
        foreach (GameObject obj in SkillList)
        {
            SkillInterfaceList.Add(obj.GetComponent<ISkill>());
        }
    }

    public void UseSkill(int slot)
    {
        if (slot < 0 || slot >= SkillList.Count) return;
        
        SkillInterfaceList[slot].Execute();
        //PlayerManager.Instance.PlayerState = EPlayerState.Skill; // 스킬에서 직접 바꿔줌
    }
}
