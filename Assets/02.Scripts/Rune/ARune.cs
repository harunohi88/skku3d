using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class ARune : MonoBehaviour
{
    public int TID;
    public Sprite Sprite;
    public float TierValue;
    public string RuneDescription;
    private int _currentTier;

    private RuneData _data;
    
    private void Start()
    {
        if (_data == null) LoadData();
        Init(1);
    }

    public void Init(int tier)
    {
        LoadData();

        _currentTier = tier;
        TierValue = _data.TierList[_currentTier - 1];
        RuneDescription = _data.RuneDescription;
        RuneDescription = RuneDescription.Replace("N", _data.RuneDescription.ToString());
    }

    //-1
    //0
    //1
    //2
    //3

    public abstract void EquipRune(int skillIndex);
    public abstract void UnEquipRune(int skillIndex);
    public abstract void OnSkill();

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
