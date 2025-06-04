using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerStat : MonoBehaviour
{
    public Dictionary<EStatType, Stat> StatDictionary = new Dictionary<EStatType, Stat>();
    public Action OnDictionaryLoaded;
    
    private void Awake()
    {
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
    
    public void StatUpgrade(EStatType statType)
    {
        if (PlayerManager.Instance.PlayerLevel.TryConsumePoints())
        {
            StatDictionary[statType].LevelUp();
        }
    }
}
