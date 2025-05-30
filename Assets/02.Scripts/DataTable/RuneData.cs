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

    ///<summary>룬 발동 타입</summary>
    public readonly string RuneTriggerType;

    ///<summary>룬 효과 타입</summary>
    public readonly string RuneEffectType;

    ///<summary>스탯 변경 타입</summary>
    public readonly string StatModifierType;

    ///<summary>티어1 값</summary>
    private readonly float Tier1;

    ///<summary>티어2 값</summary>
    private readonly float Tier2;

    ///<summary>티어3 값</summary>
    private readonly float Tier3;

    ///<summary>시간</summary>
    public readonly float Time;

    ///<summary>체력 퍼센트</summary>
    public readonly float HealthPercent;

    ///<summary>치명타 확률</summary>
    public readonly float CriticalChance;

    ///<summary>거리</summary>
    public readonly float Distance;

    ///<summary>데미지 퍼센트</summary>
    public readonly float DamagePercent;

    ///<summary>발동 확률</summary>
    public readonly float Probability;

    ///<summary>Tier 리스트</summary>
    public readonly List<float> TierList = new List<float>();
    public RuneData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        int runename = reader.ReadInt32();
        RuneName = Encoding.UTF8.GetString(reader.ReadBytes(runename));
        RuneType = (RuneType)reader.ReadInt32();
        int runedescription = reader.ReadInt32();
        RuneDescription = Encoding.UTF8.GetString(reader.ReadBytes(runedescription));
        int runetriggertype = reader.ReadInt32();
        RuneTriggerType = Encoding.UTF8.GetString(reader.ReadBytes(runetriggertype));
        int runeeffecttype = reader.ReadInt32();
        RuneEffectType = Encoding.UTF8.GetString(reader.ReadBytes(runeeffecttype));
        int statmodifiertype = reader.ReadInt32();
        StatModifierType = Encoding.UTF8.GetString(reader.ReadBytes(statmodifiertype));
        Tier1 = reader.ReadSingle();
        Tier2 = reader.ReadSingle();
        Tier3 = reader.ReadSingle();
        Time = reader.ReadSingle();
        HealthPercent = reader.ReadSingle();
        CriticalChance = reader.ReadSingle();
        Distance = reader.ReadSingle();
        DamagePercent = reader.ReadSingle();
        Probability = reader.ReadSingle();

        LinkTable();
    }

    public void LinkTable()
    {
        TierList.Add(Tier1);
        TierList.Add(Tier2);
        TierList.Add(Tier3);
    }
}
