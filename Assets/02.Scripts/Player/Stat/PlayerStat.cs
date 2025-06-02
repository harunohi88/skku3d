using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerStat : MonoBehaviour
{
    public Dictionary<EStatType, Stat> StatDictionary;
    public Action OnDictionaryLoaded;
    
    private void Awake()
    {
        StatDictionary = new Dictionary<EStatType, Stat>();
        Global.Instance.OnDataLoaded += LoadData;
    }

    private void Start()
    {
        UIEventManager.Instance.OnStatUpgrade += StatUpgrade;
    }

    private void LoadData()
    {
        ReadOnlyList<PlayerStatData> data = DataTable.Instance.GetPlayerStatDataList();

        foreach (PlayerStatData statData in data)
        {
            StatDictionary[statData.StatType] = new Stat(
                statData.BaseAmount,
                statData.CanLevelUp,
                statData.IncreaseAmount);
        }
        OnDictionaryLoaded?.Invoke();
        UIEventManager.Instance.OnDisplayStatChanged?.Invoke(new StatSnapshot());
    }
    
    // public StatSnapshot CreateSnapshot()
    // {
    //     return new StatSnapshot
    //     {
    //         TotalStats = PlayerManager.Instance.PlayerStat.StatDictionary.ToDictionary(
    //             pair => pair.Key,
    //             pair => pair.Value.TotalStat
    //         )
    //     };
    // }

    public void StatUpgrade(EStatType statType)
    {
        if (PlayerManager.Instance.PlayerLevel.TryConsumePoints())
        {
            StatDictionary[statType].LevelUp();
        }
    }
}
