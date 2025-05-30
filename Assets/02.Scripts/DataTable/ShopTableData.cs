// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ShopTableData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>스테이지</summary>
    public readonly int Stage;

    ///<summary>티어 1 확률</summary>
    private readonly float Tier1Rate;

    ///<summary>티어 2 확률</summary>
    private readonly float Tier2Rate;

    ///<summary>티어 3 확률</summary>
    private readonly float Tier3Rate;

    ///<summary>TierRate 리스트</summary>
    public readonly List<float> TierRateList = new List<float>();
    public ShopTableData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        Stage = reader.ReadInt32();
        Tier1Rate = reader.ReadSingle();
        Tier2Rate = reader.ReadSingle();
        Tier3Rate = reader.ReadSingle();

        LinkTable();
    }

    public void LinkTable()
    {
        TierRateList.Add(Tier1Rate);
        TierRateList.Add(Tier2Rate);
        TierRateList.Add(Tier3Rate);
    }
}
