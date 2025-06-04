using UnityEngine;
using System.Collections.Generic;

public class Stat
{
    public float TotalStat => _baseStat + _baseStat * _multiply + _add;
    public int Level;
    public bool CanLevelUp;

    private float _baseStat;
    private float _increasePerGap;
    private int _increaseGap;
    private float _multiply;
    private float _add;

    public Stat(float baseStat)
    {
        Level = 0;
        _baseStat = baseStat;
        CanLevelUp = false;
        _increasePerGap = 0f;
        _increaseGap = 1;
    }

    public Stat(float baseStat, bool canLevelUp, float increasePerGap, int increaseGap = 1)
    {
        Level = 0;
        _baseStat = baseStat;
        CanLevelUp = canLevelUp;
        _increasePerGap = increasePerGap;
        _increaseGap = increaseGap;
    }

    public void LevelUp()
    {
        if (!CanLevelUp) return;
        
        ++Level;
        if (Level % _increaseGap == 0)
        {
            _baseStat += _increasePerGap;
            UIEventManager.Instance.OnDisplayStatChanged(new StatSnapshot());
        }
    }

    public void AddBuff(StatBuff buff)
    {
        if (buff.BuffType == EBuffType.Add)
        {
            _add += buff.BuffValue;
            UIEventManager.Instance.OnDisplayStatChanged(new StatSnapshot());
        }
        else if (buff.BuffType == EBuffType.Multiply)
        {
            _multiply += buff.BuffValue;
            UIEventManager.Instance.OnDisplayStatChanged(new StatSnapshot());
        }
    }

    public void RemoveBuff(StatBuff buff)
    {
        if (buff.BuffType == EBuffType.Add)
        {
            _add -= buff.BuffValue;
            UIEventManager.Instance.OnDisplayStatChanged(new StatSnapshot());
        }
        else if (buff.BuffType == EBuffType.Multiply)
        {
            _multiply -= buff.BuffValue;
            UIEventManager.Instance.OnDisplayStatChanged(new StatSnapshot());
        }
    }
}
