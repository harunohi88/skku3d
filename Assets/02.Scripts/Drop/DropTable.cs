using System.Collections.Generic;
using UnityEngine;

public class DropTable : MonoBehaviour
{

    [Header("티어별 드랍 확률")]
    public float InitTier1DropRate;
    public float InitTier2DropRate;
    public float InitTier3DropRate;
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
    [SerializeField]
    private float _goldDropRate;
    private float _goldDropRateChange;
    public float GoldDropRateChange => _goldDropRateChange;

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
        _tier3DropRate = InitTier3DropRate;
        _goldDropRate = InitGoldDropRate;
    }

    // 시간에 따른 드랍 확류 변화
    public void TimeUpdate()
    {
        ChangeTierDropRate();
        ChangeGoldDropRate();
    }

    // 티어 드랍 확률 변화
    public void ChangeTierDropRate()
    {
        _tier1DropRate -= TierDropRateChange[0];
        _tier2DropRate += TierDropRateChange[1];
        _tier3DropRate += TierDropRateChange[2];
    }

    // 골드 드랍 확률 변화
    public void ChangeGoldDropRate()
    {
        _goldDropRate -= GoldDropRateChange;
    }
}
