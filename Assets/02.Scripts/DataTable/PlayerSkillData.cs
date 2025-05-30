// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class PlayerSkillData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>스킬 타입</summary>
    public readonly ESkillType SkillType;

    ///<summary>스태미나 소모량</summary>
    public readonly int Stamina;

    ///<summary>데미지 배율</summary>
    public readonly float DamageMuiltiplier;

    ///<summary>쿨타임</summary>
    public readonly float Cooldown;

    public PlayerSkillData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        SkillType = (ESkillType)reader.ReadInt32();
        Stamina = reader.ReadInt32();
        DamageMuiltiplier = reader.ReadSingle();
        Cooldown = reader.ReadSingle();
    }
}
