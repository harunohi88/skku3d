using Unity.Profiling;
using UnityEngine;
using DG.Tweening;

public class DropTable : BehaviourSingleton<DropTable>
{

    [Header("룬 드랍")]
    public float RuneDropRate;
    public float Tier1DropRate;
    public float Tier2DropRate;
    public float Tier3DropRate;

    [Header("골드 드랍")]
    // 골드 드랍 양
    public int GoldAmount;

    // 룬 데이터 시작 TID
    public const int RUNE_DATA_TID_MIN = 10000;

    [Header("경험치 드랍")]
    public int ExpAmount;

    [Header("드랍 아이템 프리펩")]
    public GameObject RunePrefab;
    public GameObject CoinPrefab;
    public GameObject ExpPrefab;

    private void Awake()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
    }

    public void Drop(EnemyType enemyType, Vector3 position)
    {
       int stage = GameManager.Instance.GetCurrentStage();
        int tid = 10000;
        switch(stage)
        {
            case 1:
                tid = 10000;
                break;
            case 2:
                tid = 20000;
                break;
            case 3:
                tid = 30000;
                break; 
        }

        switch(enemyType)
        {
            case EnemyType.Basic:
                tid += 0;
                break;
            case EnemyType.Elite:
                tid += 1;
                break;
            case EnemyType.Boss:
                tid += 2;
                break;
        }
        // 스테이지와 적 타입에 따른 드랍 테이블 데이터 가져오기
        var dropTableData = DataTable.Instance.GetDropTableData(tid);
        
        // 룬 드랍
        RuneDropRate = dropTableData.RuneDropRate;
        Tier1DropRate = dropTableData.TierDropRateList[0];
        Tier2DropRate = dropTableData.TierDropRateList[1];
        Tier3DropRate = dropTableData.TierDropRateList[2];


        //if (Random.value < RuneDropRate)
        //{
            DropRandomRune(position, enemyType);
        //}

        // 골드 드랍
        GoldAmount = Random.Range(dropTableData.MinCoin, dropTableData.MaxCoin + 1);
        DropRandomGold(position, GoldAmount);

        // 경험치 드랍
        ExpAmount = dropTableData.Exp;
        DropExp(position, ExpAmount);
    }

    public void Drop(EnemyType enemyType, Vector3 position, int runeTier)
    {
        int stage = GameManager.Instance.GetCurrentStage();
        int tid = 10000;
        switch (stage)
        {
            case 1:
                tid = 10000;
                break;
            case 2:
                tid = 20000;
                break;
            case 3:
                tid = 30000;
                break;
        }

        switch (enemyType)
        {
            case EnemyType.Basic:
                tid += 0;
                break;
            case EnemyType.Elite:
                tid += 1;
                break;
            case EnemyType.Boss:
                tid += 2;
                break;
        }
        // 스테이지와 적 타입에 따른 드랍 테이블 데이터 가져오기
        var dropTableData = DataTable.Instance.GetDropTableData(tid);

        // 룬 드랍
        RuneDropRate = dropTableData.RuneDropRate;
        Tier1DropRate = dropTableData.TierDropRateList[0];
        Tier2DropRate = dropTableData.TierDropRateList[1];
        Tier3DropRate = dropTableData.TierDropRateList[2];


        DropRandomRune(position, enemyType, runeTier);

        // 골드 드랍
        GoldAmount = Random.Range(dropTableData.MinCoin, dropTableData.MaxCoin + 1);
        DropRandomGold(position, GoldAmount);

        // 경험치 드랍
        ExpAmount = dropTableData.Exp;
        DropExp(position, ExpAmount);
    }

    /// <summary> 랜덤 룬 드랍 </summary>
    public void DropRandomRune(Vector3 position, EnemyType enemyType)
    {
        // 랜덤 티어
        int tier = 1;
        float randomFloat = Random.value;

        if(randomFloat < Tier3DropRate)
        {
            // 티어3
            tier = 3;
        }
        else if(randomFloat < Tier2DropRate)
        {
            // 티어2
            tier = 2;
        }
        else
        {
            // 티어1
            tier = 1;
        }

        int runeTID = Random.Range(RUNE_DATA_TID_MIN, RUNE_DATA_TID_MIN + DataTable.Instance.GetRuneDataList().Count);

        if(RunePrefab != null)
        {
            Item item = Instantiate(RunePrefab, position, Quaternion.identity).GetComponent<Item>();

            Rune rune = new Rune(runeTID, tier);
            // 룬 초기화
            item.Init(tier, 0, rune, EItemType.Rune);
        }
    }

    public void DropRandomRune(Vector3 position, EnemyType enemyType, int runeTier)
    {
        // 랜덤 티어
        int tier = runeTier;

        int runeTID = Random.Range(RUNE_DATA_TID_MIN, RUNE_DATA_TID_MIN + DataTable.Instance.GetRuneDataList().Count);

        if (RunePrefab != null)
        {
            Item item = Instantiate(RunePrefab, position, Quaternion.identity).GetComponent<Item>();

            Rune rune = new Rune(runeTID, tier);
            // 룬 초기화
            item.Init(tier, 0, rune, EItemType.Rune);
        }
    }

    // TODO: 각 골드, 경험치 클래스 만들어서 amount 지정 해줘야함

    /// <summary> 랜덤 골드 드랍 </summary>
    private void DropRandomGold(Vector3 position, int amount)
    {
        // 골드 인스턴스
        if(CoinPrefab != null)
        {
            Item item = Instantiate(CoinPrefab, position, Quaternion.identity).GetComponent<Item>();

            // 코인 값 초기화
            item.Init(1, amount, null, EItemType.Coin);
        }
    }

    private void DropExp(Vector3 position, int amount)
    {
        // 경험치 인스턴스
        if(ExpPrefab != null)
        {
            Item item = Instantiate(ExpPrefab, position, Quaternion.identity).GetComponent<Item>();

            // 경험치 값 초기화
            item.Init(1, amount, null, EItemType.Exp);
        }
        Debug.Log($"Dropped Exp {amount}");
    }
}
