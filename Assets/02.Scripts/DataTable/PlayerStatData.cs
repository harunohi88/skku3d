// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class PlayerStatData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>스탯 타입</summary>
    public readonly EStatType StatType;

    ///<summary>기초량</summary>
    public readonly float BaseAmount;

    ///<summary>레벨 당 증가량</summary>
    public readonly float IncreaseAmount;

    ///<summary>레벨업 가능 여부</summary>
    public readonly bool CanLevelUp;

    public PlayerStatData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        StatType = (EStatType)reader.ReadInt32();
        BaseAmount = reader.ReadSingle();
        IncreaseAmount = reader.ReadSingle();
        CanLevelUp = reader.ReadBoolean();
    }
}
