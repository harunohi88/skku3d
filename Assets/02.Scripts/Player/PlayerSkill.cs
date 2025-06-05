using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public List<ISkill> SkillList = new List<ISkill>();
    public ISkill CurrentSkill;
    public GameObject Model;
    public bool IsTargeting = false;
    
    private PlayerManager _playerManager;

    private void Start()
    {
        _playerManager = PlayerManager.Instance;
        UIEventManager.Instance.OnSKillLevelUp += SkillLevelUp;
        SkillList = GetComponents<ISkill>().ToList();
        foreach (ISkill skill in SkillList)
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

    private void SkillLevelUp(int index)
    {
        Debug.Log($"Skill {index} LevelUp");
        SkillList[index].LevelUp();
    }

    public void UseSkill(int slot)
    {
        if (slot < 0 || slot >= SkillList.Count) return;

        if (!IsTargeting && CurrentSkill != null) return;
        
        _playerManager.PlayerRotate.InstantLookAtMouse();
        SkillList[slot]?.Execute();
    }

    public void AddRune(int slot, Rune rune)
    {
        SkillList[slot].EquipRune(rune);
    }

    public void RemoveRune(int slot)
    {
        SkillList[slot].UnequipRune();
    }

    public void Cancel()
    {
        CurrentSkill?.Cancel();
        CurrentSkill = null;
        IsTargeting = false;
    }
}
