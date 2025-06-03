using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class Rune
{
    public int TID;
    public Sprite Sprite;
    public float TierValue;
    public string RuneDescription;

    public string Name;
    public int CurrentTier;
    private RuneData _data;
    private List<ARuneEquip> _equipList;
    private List<ARuneTrigger> _triggerList;
    private List<ARuneEffect> _effectList;

    public Rune(int tid, int tier)
    {
        TID = tid;

        Init(tier);
    }

    public void Init(int tier)
    {
        LoadData();

        Name = _data.RuneName;
        CurrentTier = tier;
        TierValue = _data.TierList[CurrentTier - 1];
        RuneDescription = _data.RuneDescription;
        RuneDescription = RuneDescription.Replace("N", TierValue.ToString(CultureInfo.CurrentCulture));
        InitEquipList();
        InitTriggerList();
        InitEffectList();
    }

    public void InitEquipList()
    {
        List<string> EquipNameList = _data.RuneEquipType.Split(", ").ToList();
        _equipList = new List<ARuneEquip>();

        foreach (string equipName in EquipNameList)
        {
            ARuneEquip equip = RuneEquipFactory.Instance.CreateRuneEquip(equipName);
            if (equip != null)
            {
                equip.Initialize(_data, CurrentTier);
                _equipList.Add(equip);
            }
        }
    }
    
    public void InitTriggerList()
    {
        List<string> triggerNameList = _data.RuneTriggerType.Split(", ").ToList();
        _triggerList = new List<ARuneTrigger>();
        
        foreach (string triggerName in triggerNameList)
        {
            ARuneTrigger trigger = RuneTriggerFactory.Instance.CreateRuneTrigger(triggerName);
            if (trigger != null)
            {
                trigger.Initialize(_data);
                _triggerList.Add(trigger);
            }
        }
    }

    public void InitEffectList()
    {
        List<string> effectNameList = _data.RuneEffectType.Split(", ").ToList();
        _effectList = new List<ARuneEffect>();
        
        foreach (string effectName in effectNameList)
        {
            ARuneEffect effect = RuneEffectFactory.Instance.CreateRuneEffect(effectName);
            if (effect != null)
            {
                effect.Initialize(_data, CurrentTier);
                _effectList.Add(effect);
            }
        }
    }

    public void EquipRune()
    {
        foreach (ARuneEquip equip in _equipList)
        {
            equip.OnEquip();
        }
    }

    public void UnequipRune()
    {
        foreach (ARuneEquip equip in _equipList)
        {
            equip.OnUnequip();
        }
    }
    
    public void EquipRune(int skillIndex)
    {
        // 만약에 스킬의 스탯에 룬 적용하려면 일단 OnEquip에 skillIndex 전달하고
        // 룬 내부 변수에서 어디에 적용되는 버프인지 판단하는 로직이 필요할듯?
        // 버프 매니저에서도 스킬에 버프를 적용하는 로직이 필요할수도 있음
        foreach (ARuneEquip equip in _equipList)
        {
            equip.OnEquip();
        }
    }

    public void UnequipRune(int skillIndex)
    {
        foreach (ARuneEquip equip in _equipList)
        {
            equip.OnUnequip();
        }
    }

    public bool CheckTrigger(RuneExecuteContext context)
    {
        if (_triggerList == null || _triggerList.Count == 0)
        {
            Debug.Log("상시 발동 하는 룬");
            return true;
        }

        foreach (ARuneTrigger trigger in _triggerList)
        {
            // 모든 트리거에서 True 반환시 동작
            if (!trigger.Trigger(context))
            {
                return false;
            }
        }
        return true;
    }
    
    public void ApplyEffect(RuneExecuteContext context, ref Damage damage)
    {
        if (_effectList == null || _effectList.Count == 0)
        {
            Debug.Log("상시 발동 하는 룬");
            return;
        }

        foreach (ARuneEffect effect in _effectList)
        {
            effect.ApplyEffect(context, ref damage);
        }
    }

    public void OnSkill(RuneExecuteContext context, ref Damage damage)
    {
        
    }

    public bool TryUpgrade()
    {
        if (CurrentTier >= _data.TierList.Count) return false;

        CurrentTier++;
        Init(CurrentTier);
        return true;
    }

    private void LoadData()
    {
        if (TID == 0)
        {
            Debug.Log("TID를 넣어주세요");
            return;
        }

        _data = DataTable.instance.GetRuneData(TID);
    }
}
