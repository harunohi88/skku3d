// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class PlayerSkillUpgradeData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>스킬 타입</summary>
    public readonly ESkillType SkillType;

    ///<summary>업그레이드 타입</summary>
    public readonly SkillUpgradeType UpgradeType;

    ///<summary>업그레이드 량</summary>
    public readonly float Amount;

    ///<summary>레벨 단위</summary>
    public readonly int PerLevel;

    public PlayerSkillUpgradeData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        SkillType = (ESkillType)reader.ReadInt32();
        UpgradeType = (SkillUpgradeType)reader.ReadInt32();
        Amount = reader.ReadSingle();
        PerLevel = reader.ReadInt32();
    }
}
