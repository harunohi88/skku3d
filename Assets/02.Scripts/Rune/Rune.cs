using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
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
