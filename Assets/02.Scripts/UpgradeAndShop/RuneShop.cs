using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneShop : MonoBehaviour
{
    [SerializeField] private BasicInventory _inventory;     // 인벤토리 참조

    public List<Rune> RuneList = new List<Rune>(6);         // 룬 리스트

    // 일단은 1티어 가격
    private List<int> _runeCostList = new List<int>(6);     // 룬 가격 리스트

    public int ItemCount = 6;
    [SerializeField]
    private int _rerollCost = 100;
    
    public const int RUNE_MIN_TID = 10000;
    public const int RUNE_COUNT = 20;

    public Action<int, Sprite, int> OnRuneUpdated;
    public Action<int> OnItemSoldout;
    public Action<int> OnCreateRune;
    public Action<int> OnReroll;


    private void Start()
    {
        CreateRuneList();
    }

    /// <summary>
    /// 룬 구매하고 인벤토리에 추가
    /// </summary>
    public void BuyRune(int index)
    {
        if(CurrencyManager.Instance.TrySpendGold(_runeCostList[index]))
        {
            // TODO: 룬을 인벤토리에 추가
            Debug.Log($"룬 구매: {RuneList[index]} 가격: {_runeCostList[index]}");
            _inventory.AddItem(RuneList[index]);
            // 룬 구매 성공시 판매 완료 이벤트 실행
            OnItemSoldout?.Invoke(index);
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void Reroll()
    {
        if(!CurrencyManager.Instance.TrySpendGold(_rerollCost))
        {
            Debug.Log("골드가 부족합니다.");
            return;
        }
        
        RuneList.Clear();
        _runeCostList.Clear();

        _rerollCost *= 2;
        CreateRuneList();
    }


    private void CreateRuneList()
    {
        if(RuneList.Count > 0)
        {
            RuneList.Clear();
            _runeCostList.Clear();
        }

        for(int i=0; i< ItemCount; i++)
        {
            // TODO: 일단 티어를 전부 1로 설정
            // 나중에는 티어가 상황에 따라 바뀔 거 같음
            // 가격도 전부 100으로 설정
            int randomTID = UnityEngine.Random.Range(RUNE_MIN_TID, RUNE_MIN_TID + RUNE_COUNT);
            Debug.Log($"룬 생성: {randomTID}");
            Rune rune = new Rune(randomTID, 1);
            RuneList.Add(rune);
            _runeCostList.Add(100);
            
            // 룬 리스트 만들고 UI 업데이트
            OnReroll?.Invoke(_rerollCost);
            OnRuneUpdated?.Invoke(i, rune.Sprite, _runeCostList[i]);
            OnCreateRune?.Invoke(i);
        }
    }
}
