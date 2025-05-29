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

    private void Update()
    {
        if (IsTargeting)
        {
            PlayerManager.Instance.PlayerState = EPlayerState.Targeting;
        }
    }

    public void UseSkill(int slot)
    {
        Debug.Log("Use Skill");
        Debug.Log(CurrentSkill);
        if (slot < 0 || slot >= _skillList.Count) return;

        if (!IsTargeting && CurrentSkill != null) return;
        
        _skillList[slot]?.Execute();
    }

    public void AddRune(int slot, Rune rune)
    {
        _skillList[slot].EquipRune(rune);
    }

    public void Cancel()
    {
        CurrentSkill?.Cancel();
        CurrentSkill = null;
        IsTargeting = false;
    }
}
