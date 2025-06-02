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
    
    private PlayerManager _playerManager;

    public void Start()
    {
        _playerManager = PlayerManager.Instance;
        _skillList = GetComponents<ISkill>().ToList();
        UIEventManager.Instance.OnSKillLevelUp += SkillLevelUp;
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

    private void SkillLevelUp(int index)
    {
        Debug.Log($"Skill {index} LevelUp");
        _skillList[index].LevelUp();
    }

    public void UseSkill(int slot)
    {
        if (slot < 0 || slot >= _skillList.Count) return;

        if (!IsTargeting && CurrentSkill != null) return;
        
        _playerManager.PlayerRotate.InstantLookAtMouse();
        _skillList[slot]?.Execute();
    }

    public void AddRune(int slot, Rune rune)
    {
        _skillList[slot].EquipRune(rune);
    }

    public void RemoveRune(int slot)
    {
        _skillList[slot].UnequipRune();
    }

    public void Cancel()
    {
        CurrentSkill?.Cancel();
        CurrentSkill = null;
        IsTargeting = false;
    }
}
