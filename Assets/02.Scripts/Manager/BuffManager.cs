using System;
using UnityEngine;
using System.Collections.Generic;

public class BuffManager : BehaviourSingleton<BuffManager>
{
    private List<StatBuff> _activeBuffList;
    private Dictionary<int, StatBuff> _activeBuffDictionary;

    private void Awake()
    {
        _activeBuffList = new List<StatBuff>();
        _activeBuffDictionary = new Dictionary<int, StatBuff>();
    }

    private void Start()
    {
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
            PlayerManager.Instance.PlayerStat.StatDictionary[buff.BuffStatType].AddBuff(buff);
        }
    }

    public void RemoveBuff(StatBuff buff)
    {
        PlayerManager.Instance.PlayerStat.StatDictionary[buff.BuffStatType].RemoveBuff(buff);
        _activeBuffList.Remove(buff);
        _activeBuffDictionary.Remove(buff.SourceTid);
    }

    public void Update()
    {
        for (int i = _activeBuffList.Count - 1; i >= 0; --i)
        {
            StatBuff buff = _activeBuffList[i];
            if (!buff.IsValid(Time.deltaTime))
            {
                Debug.LogWarning("Remove Buff");
                _activeBuffList.RemoveAt(i);
                _activeBuffDictionary.Remove(buff.SourceTid);
                PlayerManager.Instance.PlayerStat.StatDictionary[buff.BuffStatType].RemoveBuff(buff);
            }
        }
    }
}
