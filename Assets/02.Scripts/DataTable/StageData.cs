// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class StageData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>스테이지</summary>
    public readonly int Stage;

    ///<summary>최소 스폰</summary>
    public readonly int MinSpawnAmount;

    ///<summary>최대 스폰</summary>
    public readonly int MaxSpawnAmount;

    ///<summary>기본 적 기초 데미지</summary>
    public readonly float BasicBaseDamage;

    ///<summary>엘리트 적 기초 데미지</summary>
    public readonly float EliteBaseDamage;

    ///<summary>기본 적 기초 체력</summary>
    public readonly float BasicBaseHealth;

    ///<summary>엘리트 적 기초 체력</summary>
    public readonly float EliteBaseHealth;

    public StageData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        Stage = reader.ReadInt32();
        MinSpawnAmount = reader.ReadInt32();
        MaxSpawnAmount = reader.ReadInt32();
        BasicBaseDamage = reader.ReadSingle();
        EliteBaseDamage = reader.ReadSingle();
        BasicBaseHealth = reader.ReadSingle();
        EliteBaseHealth = reader.ReadSingle();
    }
}
