using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    private List<ISkill> _skillList = new List<ISkill>();
    public ISkill CurrentSkill;
    public GameObject Model;
    public bool IsTargeting = false;

    public void Start()
    {
        _skillList = GetComponents<ISkill>().ToList();
        foreach (ISkill skill in _skillList)
        {
            skill.Initialize();
        }
    }

    public void UseSkill(int slot)
    {
        if (slot < 0 || slot >= _skillList.Count) return;
        
        
        // 스킬 타겟팅중 발생하는 입력을 처리할 로직이 필요함
        _skillList[slot]?.Execute();
    }

    public void Cancel()
    {
        CurrentSkill?.Cancel();
        IsTargeting = false;
    }
}
