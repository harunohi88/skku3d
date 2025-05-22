using System.Collections.Generic;
using Rito.InventorySystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DropTable : MonoBehaviour
{

    [Header("티어별 드랍 확률")]
    public float InitTier1DropRate = 0.6f;
    public float InitTier2DropRate = 0.3f;
    //public float InitTier3DropRate = 0.1f;
    [SerializeField]
    private float _tier1DropRate;
    [SerializeField]
    private float _tier2DropRate;
    [SerializeField]
    private float _tier3DropRate;

    // 시간당 드랍 확률 변화
    // ex: 티어1은 감소, 티어2, 티어3는 증가
    public float[] TierDropRateChange = new float[3];

    [Header("골드 드랍 확률")]
    public float InitGoldDropRate;
    // 골드 드랍 양
    public int DropGoldAmount;
    [SerializeField]
    private float _goldDropRate;
    private float _goldDropRateChange;
    public float GoldDropRateChange => _goldDropRateChange;

    // 룬이 추가될 인벤토리
    public Inventory RuneInventory;
    // 룬 데이터 시작 TID
    public const int RUNE_DATA_TID_MIN = 10000;


    // 룬 데이터 시트에서 룬 개수
    public int RuneDataCount = 2;

    // TODO: 룬 드랍
    // 룬은 정적 룬과 동적룬이 확률에 따라 등장한다.
    // 정적 룬의 경우 티어 드랍 확률에 따라 등장한다.
    // 동적 룬도 정적 룬과 같다
    // 몬스터 타입에 따라 이 티어 드랍 확률리 변한다.
    // 골드는 확률에 따라 드랍한다.

    private void Awake()
    {
        _tier1DropRate = InitTier1DropRate;
        _tier2DropRate = InitTier2DropRate;
        _tier3DropRate = 1 - InitTier1DropRate - InitTier2DropRate;
        _goldDropRate = InitGoldDropRate;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DropRandomRune();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    /// <summary> 랜덤 룬 드랍 </summary>
    public void DropRandomRune()
    {
        int randomRuneTier = GetRandomTier();

        int randomRuneDataTIDPlus = Random.Range(0, RuneDataCount);
        RuneData runeData = DataTable.Instance.GetRuneData(RUNE_DATA_TID_MIN + randomRuneDataTIDPlus);
        RuneItemData runeItemData = RuneItemConverter.ConvertToItemData(runeData, randomRuneTier);

        // TODO: 지금은 인벤토리로 추가하고 있는데
        // Return을 RuneItemData로 바꿔야 할듯
        // 그리고 아이템 자체에서 획득하면 인벤토리로 추가하도록 하는게 나을듯?
        RuneInventory.Add(runeItemData, 1);
    }

    /// <summary> 랜덤 티어 반환 </summary>
    private int GetRandomTier()
    {
        int randomRuneTier = 1;
        // 룬 램덤 티어
        float randomFloat = Random.Range(0f, 1f);
        Debug.Log($"randomFloat: {randomFloat}");
        if (randomFloat < _tier1DropRate)
        {
            // 티어1
            randomRuneTier = 1;
        }
        else if (randomFloat < _tier1DropRate + _tier2DropRate)
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

    /// <summary> 랜덤 골드 드랍 </summary>
    public int DropRandomGold()
    {
        float randomFloat = Random.Range(0f, 1f);
        if(randomFloat < _goldDropRate)
        {
            // 골드 드랍
            return DropGoldAmount;
        }
        return 0;
    }

    /// <summary> 시간에 따른 드랍 확류 변화</summary>
    public void TimeUpdate()
    {
        ChangeTierDropRate();
        ChangeGoldDropRate();
    }

    /// <summary> 티어 드랍 확률 변화</summary>
    public void ChangeTierDropRate()
    {
        _tier1DropRate -= TierDropRateChange[0];
        _tier2DropRate += TierDropRateChange[1];
        _tier3DropRate += TierDropRateChange[2];
    }

    /// <summary> 골드 드랍 확률 변화</summary>
    public void ChangeGoldDropRate()
    {
        _goldDropRate -= GoldDropRateChange;
    }
}
