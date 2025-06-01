using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneShop : MonoBehaviour
{
    [SerializeField] private BasicAllInventory _inventory;     // 인벤토리 참조

    public List<Rune> RuneList = new List<Rune>(6);         // 룬 리스트

    // 일단은 1티어 가격
    private List<int> _runeCostList = new List<int>(6);     // 룬 가격 리스트

    public int ItemCount = 6;
    [SerializeField]
    private int _rerollCost = 50;
    private int _rerollCostIncrease = 200;
    
    public const int RUNE_MIN_TID = 10000;
    public const int REROLL_TID = 10004;
    public const int RUNE_COUNT = 20;

    public Action<int, Sprite, int> OnRuneUpdated;
    public Action<int> OnItemSoldout;
    public Action<int> OnCreateRune;
    public Action<int> OnReroll;


    // TODO: 스테이지 바뀔때마다 조건을 바꿔서 다시 리롤을 해줘야 한다.

    private void Start()
    {
        CreateRuneList();

        CurrencyData currencyData = DataTable.Instance.GetCurrencyData(REROLL_TID);
        _rerollCost = currencyData.BaseAmount;
        _rerollCostIncrease = currencyData.AddAmount;
    }

    /// <summary>
    /// 룬 구매하고 인벤토리에 추가
    /// </summary>
    public void BuyRune(int index)
    {
        if(CurrencyManager.Instance.TrySpendGold(_runeCostList[index]))
        {
            AudioManager.Instance.PlayUIAudio(UIAudioType.ItemPurchase);

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
            AudioManager.Instance.PlayUIAudio(UIAudioType.Fail);

            Debug.Log("골드가 부족합니다.");
            return;
        }

        AudioManager.Instance.PlayUIAudio(UIAudioType.ReRoll);

        RuneList.Clear();
        _runeCostList.Clear();

        _rerollCost += _rerollCostIncrease;
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
            // 테스트트
            //int stage = GameManager.Instance.GetCurrentStage();
            int stage = 3;
            // 스테이지에 따른 상점 테이블 데이터 가져오기
            ShopTableData shopTableData = DataTable.Instance.GetShopTableData(10000 + stage - 1);

            // 상점에 나올 룬 랜덤 티어 결정
            int tier = 1;
            float tierRandomValue = UnityEngine.Random.value;
            if(tierRandomValue < shopTableData.TierRateList[2])
            {
                tier = 3;
            }
            else if(tierRandomValue < shopTableData.TierRateList[1])
            {
                tier = 2;
            }
            else if(tierRandomValue < shopTableData.TierRateList[0])
            {
                tier = 1;
            }

            // 상점에 나올 룬 랜덤 TID 결정
            int randomTID = UnityEngine.Random.Range(RUNE_MIN_TID, RUNE_MIN_TID + DataTable.Instance.GetRuneDataList().Count);
            Debug.Log($"룬 생성: {randomTID}");
            Rune rune = new Rune(randomTID, tier);
            RuneList.Add(rune);

            // 티어에 따라 룬 가격 책정
            int currencyDataTID = 10000;
            switch(tier)
            {
                case 1:
                {
                    currencyDataTID += 1;
                    break;          
                }
                case 2:
                {
                    currencyDataTID += 2;
                    break;
                }
                case 3:
                {
                    currencyDataTID += 3;
                    break;
                }
            }
            CurrencyData currencyData = DataTable.Instance.GetCurrencyData(currencyDataTID);
            
            _runeCostList.Add(currencyData.BaseAmount);
            
            // 룬 리스트 만들고 UI 업데이트
            OnReroll?.Invoke(_rerollCost);
            OnRuneUpdated?.Invoke(i, InventoryManager.Instance.GetSprite(randomTID), _runeCostList[i]);
            OnCreateRune?.Invoke(i);
        }
    }
}
