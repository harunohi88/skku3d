using UnityEngine;

public class CurrencyManager : BehaviourSingleton<CurrencyManager>
{
    [SerializeField]private int _currnetGold;

    // 테스트용용
    void Start()
    {
        _currnetGold = 9999;
    }

    public void AddGold(int amount)
    {
        _currnetGold += amount;
    }

    public bool TrySpendGold(int amount)
    {
        if(_currnetGold >= amount)
        {
            _currnetGold -= amount;
            return true;
        }
        return false;
    }

    public int GetCurrentGold()
    {
        return _currnetGold;
    }
}
