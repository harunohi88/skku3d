// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class RuneData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>룬 이름</summary>
    public readonly string RuneName;

    ///<summary>룬 타입</summary>
    public readonly RuneType RuneType;

    ///<summary>룬 설명</summary>
    public readonly string RuneDescription;

    ///<summary>티어1 값</summary>
    private readonly int Tier1;

    ///<summary>티어2 값</summary>
    private readonly int Tier2;

    ///<summary>티어3 값</summary>
    private readonly int Tier3;

    ///<summary>시간</summary>
    public readonly float Time;

    ///<summary>체력 퍼센트</summary>
    public readonly int HealthPercent;

    ///<summary>치명타 확률</summary>
    public readonly int CriticalChance;

    ///<summary>Tier 리스트</summary>
    public readonly List<int> TierList = new List<int>();
    public RuneData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        int runename = reader.ReadInt32();
        RuneName = Encoding.UTF8.GetString(reader.ReadBytes(runename));
        RuneType = (RuneType)reader.ReadInt32();
        int runedescription = reader.ReadInt32();
        RuneDescription = Encoding.UTF8.GetString(reader.ReadBytes(runedescription));
        Tier1 = reader.ReadInt32();
        Tier2 = reader.ReadInt32();
        Tier3 = reader.ReadInt32();
        Time = reader.ReadSingle();
        HealthPercent = reader.ReadInt32();
        CriticalChance = reader.ReadInt32();

        LinkTable();
    }

    public void LinkTable()
    {
        TierList.Add(Tier1);
        TierList.Add(Tier2);
        TierList.Add(Tier3);
    }
}
