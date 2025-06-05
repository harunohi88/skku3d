using System;
using UnityEngine;

public class CurrencyManager : BehaviourSingleton<CurrencyManager>
{
    [SerializeField]private int _currnetGold;
    public int CurrentGold => _currnetGold;

    // 골드 변경시 이벤트
    public Action<int> OnGoldChanged;

    // 테스트용용
    void Start()
    {
        OnGoldChanged?.Invoke(_currnetGold);
    }

    public void AddGold(int amount)
    {
        _currnetGold += amount;
        OnGoldChanged?.Invoke(_currnetGold);
    }

    public bool TrySpendGold(int amount)
    {
        if(_currnetGold >= amount)
        {
            _currnetGold -= amount;
            OnGoldChanged?.Invoke(_currnetGold);
            return true;
        }
        return false;
    }

    public int GetCurrentGold()
    {
        return _currnetGold;
    }
}
