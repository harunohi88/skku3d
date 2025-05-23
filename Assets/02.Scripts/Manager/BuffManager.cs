using UnityEngine;
using System.Collections.Generic;

public class BuffManager : BehaviourSingleton<BuffManager>
{
    private PlayerStat _playerStat;
    private List<StatBuff> _activeBuffList;

    public void AddBuff(StatBuff buff)
    {
        _playerStat.StatDictionary[buff.BuffStatType].AddBuff(buff);
        _activeBuffList.Add(buff);
    }

    public void Update()
    {
        for (int i = _activeBuffList.Count - 1; i >= 0; --i)
        {
            StatBuff buff = _activeBuffList[i];
            if (!buff.IsValid())
            {
                // buff의 타입을 보고 PlayerStat을 통해 접근해서 제거
            }
        }
    }
}
