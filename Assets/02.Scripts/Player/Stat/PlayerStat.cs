using UnityEngine;
using System.Collections.Generic;

public class PlayerStat : MonoBehaviour
{
    public Dictionary<EStatType, Stat> StatDictionary;
    
    private void Awake()
    {
        StatDictionary = new Dictionary<EStatType, Stat>();
        Global.Instance.OnDataLoaded += LoadData;
    }

    private void LoadData()
    {
        ReadOnlyList<PlayerStatData> data = DataTable.Instance.GetPlayerStatDataList();

        foreach (PlayerStatData statData in data)
        {
            StatDictionary[statData.StatType] = new Stat(
                statData.BaseAmount,
                statData.IncreaseAmount,
                statData.CanLevelUp);
        }
    }
}
