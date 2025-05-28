using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rune
{
    public int TID;
    public Sprite Sprite;
    public float TierValue;
    public string RuneDescription;

    private int _currentTier;
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

        _currentTier = tier;
        TierValue = _data.TierList[_currentTier - 1];
        RuneDescription = _data.RuneDescription;
        RuneDescription = RuneDescription.Replace("N", _data.RuneDescription.ToString());
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
                effect.Initialize(_data, _currentTier);
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

    public void OnSkill(RuneExecuteContext context, ref Damage damage)
    {
        
    }

    public bool TryUpgrade()
    {
        if (_currentTier >= _data.TierList.Count) return false;

        _currentTier++;
        Init(_currentTier);
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
