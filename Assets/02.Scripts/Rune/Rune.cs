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

    public int CurrentTier;
    private RuneData _data;
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

        CurrentTier = tier;
        TierValue = _data.TierList[CurrentTier - 1];
        RuneDescription = _data.RuneDescription;
        RuneDescription = RuneDescription.Replace("N", TierValue.ToString(CultureInfo.CurrentCulture));
        InitTriggerList();
        InitEffectList();
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

    public void EquipRune(int skillIndex)
    {
        
    }

    public void UnEquipRune(int skillIndex)
    {
        
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
