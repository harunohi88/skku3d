using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class StatSnapshot
{
    public Dictionary<EStatType, float> TotalStats;

    public StatSnapshot()
    {
        Dictionary<EStatType, Stat> playerStatDictionary = PlayerManager.Instance.PlayerStat.StatDictionary;
        
        TotalStats = playerStatDictionary.ToDictionary(
            pair => pair.Key,
            pair => pair.Value.TotalStat
            );
    }
}
