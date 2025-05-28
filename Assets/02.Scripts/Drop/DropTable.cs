using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DropTable : BehaviourSingleton<DropTable>
{

    [Header("룬 드랍")]
    public float BasicRuneDropRate = 0.3f;
    public float EliteRuneDropRate = 0.5f;
    public float BossRuneDropRate = 1f;
    public float BasicTier1DropRate = 0.6f;
    public float BasicTier2DropRate = 0.3f;
    //public float InitTier3DropRate = 0.1f;

    public float EliteTier1DropRate = 0.5f;
    public float EliteTier2DropRate = 0.3f;

    public float BossTier1DropRate = 0f;
    public float BossTier2DropRate = 0.7f;

    [Header("골드 드랍")]
    public float BasicGoldDropRate = 0.3f;
    public float EliteGoldDropRate = 0.5f;
    public float BossGoldDropRate = 1f;
    // 골드 드랍 양
    public int BasicDropGoldAmount;
    public int EliteDropGoldAmount;
    public int BossDropGoldAmount;


    // 룬이 추가될 인벤토리
    public Inventory RuneInventory;
    // 룬 데이터 시작 TID
    public const int RUNE_DATA_TID_MIN = 10000;

    [Header("경험치 드랍")]
    public int BasicDropExpAmount;
    public int EliteDropExpAmount;
    public int BossDropExpAmount;

    [Header("드랍 아이템 프리펩")]
    public GameObject RunePrefab;
    public GameObject CoinPrefab;
    public GameObject ExpPrefab;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DropRandomRune(new Vector3(-383.12f, 0.882762f, 417.03f), EnemyType.Basic);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void Drop(EnemyType enemyType, Vector3 position)
    {
        // TODO : 몬스터 타입에 따라
        // 몬스터 타입에 따라 경험치(는 그냥주고), 룬은 확률, 골드도 확률로 생성
        // 그 죽은 위치에 경험치, 룬, 골드 스폰 그냥 인스턴스화
        // 나머지 DropRandomRune 등등 private 은닉

        float runeDropRate = 0;
        float goldDropRate = 0;
        // 적 타입의 따라 드랍 확률 설정
        switch(enemyType)
        {
            case EnemyType.Basic:
                runeDropRate = BasicRuneDropRate;
                goldDropRate = BasicGoldDropRate;
                break;
            case EnemyType.Elite:
                runeDropRate = EliteRuneDropRate;
                goldDropRate = EliteGoldDropRate;
                break;
            case EnemyType.Boss:
                runeDropRate = BossRuneDropRate;
                goldDropRate = BossGoldDropRate;
                break;
        }

        // 룬 드랍
        if(Random.value < runeDropRate)
        {
            DropRandomRune(position, enemyType);
        }
        /* 프로토 타입때는 안씀
        if(Random.value < goldDropRate)
        {
            DropRandomGold(position);
        }

        DropExp(position);
        */
    }

    /// <summary> 랜덤 룬 드랍 </summary>
    public void DropRandomRune(Vector3 position, EnemyType enemyType)
    {
        int tier = GetRandomTier(enemyType);

        int runeTID = Random.Range(0, DataTable.Instance.GetRuneDataList().Count);

        if(RunePrefab != null)
        {
            Item item = Instantiate(RunePrefab, position, Quaternion.identity).GetComponent<Item>();

            Rune rune = new Rune(runeTID, tier);
            // 룬 초기화
            item.Init(tier, 0, rune, EItemType.Rune);
        }
    }

    /// <summary> 랜덤 티어 반환 </summary>
    private int GetRandomTier(EnemyType enemyType)
    {
        int randomRuneTier = 1;

        float tier1DropRate = 0;
        float tier2DropRate = 0;

        switch(enemyType)
        {
            case EnemyType.Basic:
                tier1DropRate = BasicTier1DropRate;
                tier2DropRate = BasicTier2DropRate;
                break;
            case EnemyType.Elite:
                tier1DropRate = EliteTier1DropRate;
                tier2DropRate = EliteTier2DropRate;
                break;
            case EnemyType.Boss:
                tier1DropRate = BossTier1DropRate;
                tier2DropRate = BossTier2DropRate;
                break;
        }

        // 룬 램덤 티어
        float randomFloat = Random.value;
        Debug.Log($"randomFloat: {randomFloat}");
        if (randomFloat < tier1DropRate)
        {
            // 티어1
            randomRuneTier = 1;
        }
        else if (randomFloat < tier1DropRate + tier2DropRate)
        {
            // 티어2
            randomRuneTier = 2;
        }
        else
        {
            // 티어3
            randomRuneTier = 3;
        }

        return randomRuneTier;
    }

    // TODO: 각 골드, 경험치 클래스 만들어서 amount 지정 해줘야함

    /// <summary> 랜덤 골드 드랍 </summary>
    private void DropRandomGold(Vector3 position)
    {
        // 골드 인스턴스
        if(CoinPrefab != null)
        {
            Item item = Instantiate(CoinPrefab, position, Quaternion.identity).GetComponent<Item>();

            // 코인 값 초기화
            // item.Init(1, 5, null, EItemType.Coin);
        }
    }

    private void DropExp(Vector3 position)
    {
        // 경험치 인스턴스
        if(ExpPrefab != null)
        {
            Item item = Instantiate(ExpPrefab, position, Quaternion.identity).GetComponent<Item>();

            // 경험치 값 초기화
            // item.Init(1, 5, null, EItemType.Exp);
        }
    }
}
