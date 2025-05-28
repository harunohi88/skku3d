using System;
using UnityEngine;
using System.Collections.Generic;

public class BuffManager : BehaviourSingleton<BuffManager>
{
    private PlayerStat _playerStat;
    private List<StatBuff> _activeBuffList;
    private Dictionary<int, StatBuff> _activeBuffDictionary;

    private void Awake()
    {
        _playerStat = PlayerManager.Instance.PlayerStat;
        _activeBuffList = new List<StatBuff>();
        _activeBuffDictionary = new Dictionary<int, StatBuff>();
    }

    public void AddBuff(StatBuff buff)
    {
        if (_activeBuffDictionary.TryGetValue(buff.SourceTid, out StatBuff existing))
        {
            existing.RefreshTime(buff.Duration);
        }
        else
        {
            _activeBuffDictionary[buff.SourceTid] = buff;
            _activeBuffList.Add(buff);
            _playerStat.StatDictionary[buff.BuffStatType].AddBuff(buff);
        }
    }

    public void Update()
    {
        for (int i = _activeBuffList.Count - 1; i >= 0; --i)
        {
            StatBuff buff = _activeBuffList[i];
            if (!buff.IsValid(Time.deltaTime))
            {
                _playerStat.StatDictionary[buff.BuffStatType].RemoveBuff(buff);
                _activeBuffList.RemoveAt(i);
            }
        }
    }
}
