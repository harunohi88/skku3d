// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class DropTableData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>스테이지</summary>
    public readonly int Stage;

    ///<summary>적 타입</summary>
    public readonly EnemyType EnemyType;

    ///<summary>코인</summary>
    public readonly int Coin;

    ///<summary>경험치</summary>
    public readonly int Exp;

    ///<summary>티어 1 룬 드랍 확률</summary>
    private readonly float Tier1DropRate;

    ///<summary>티어 2 룬 드랍 확률</summary>
    private readonly float Tier2DropRate;

    ///<summary>티어 3 룬 드랍 확률</summary>
    private readonly float Tier3DropRate;

    ///<summary>TierDropRate 리스트</summary>
    public readonly List<float> TierDropRateList = new List<float>();
    public DropTableData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        Stage = reader.ReadInt32();
        EnemyType = (EnemyType)reader.ReadInt32();
        Coin = reader.ReadInt32();
        Exp = reader.ReadInt32();
        Tier1DropRate = reader.ReadSingle();
        Tier2DropRate = reader.ReadSingle();
        Tier3DropRate = reader.ReadSingle();

        LinkTable();
    }

    public void LinkTable()
    {
        TierDropRateList.Add(Tier1DropRate);
        TierDropRateList.Add(Tier2DropRate);
        TierDropRateList.Add(Tier3DropRate);
    }
}
