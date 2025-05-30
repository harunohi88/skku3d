using UnityEngine;
using System.Collections.Generic;

public class Stat
{
    public float TotalStat => _baseStat + _baseStat * _multiply + _add;
    public bool CanLevelUp;
    
    private float _baseStat;
    private float _increaseAmountPerLevel;
    private float _multiply;
    private float _add;

    public Stat(float baseStat)
    {
        _baseStat = baseStat;
        _increaseAmountPerLevel = 0f;
        CanLevelUp = false;
    }
    
    public Stat(float baseStat, float increaseAmountPerLevel, bool canLevelUp)
    {
        _baseStat = baseStat;
        _increaseAmountPerLevel = increaseAmountPerLevel;
        CanLevelUp = canLevelUp;
    }

    public void AddBuff(StatBuff buff)
    {
        if (buff.BuffType == EBuffType.Add)
        {
            _add += buff.BuffValue;
        }
        else if (buff.BuffType == EBuffType.Multiply)
        {
            _multiply += buff.BuffValue;
        }
    }

    public void RemoveBuff(StatBuff buff)
    {
        if (buff.BuffType == EBuffType.Add)
        {
            _add -= buff.BuffValue;
        }
        else if (buff.BuffType == EBuffType.Multiply)
        {
            _multiply -= buff.BuffValue;
        }
    }

    public void LevelUp()
    {
        _baseStat += _increaseAmountPerLevel;
    }
}
