using System;
using UnityEngine;
using System.Collections.Generic;

public class BuffManager : BehaviourSingleton<BuffManager>
{
    private PlayerStat _playerStat;
    private List<StatBuff> _activeBuffList;

    private void Awake()
    {
        _playerStat = PlayerManager.Instance.PlayerStat;
        _activeBuffList = new List<StatBuff>();
    }

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
                _playerStat.StatDictionary[buff.BuffStatType].RemoveBuff(buff);
                _activeBuffList.RemoveAt(i);
            }
        }
    }
}
