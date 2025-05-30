// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class PlayerExperienceData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>레벨</summary>
    public readonly int Level;

    ///<summary>다음 레벨까지 경험치량</summary>
    public readonly int ExpToNextLevel;

    public PlayerExperienceData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        Level = reader.ReadInt32();
        ExpToNextLevel = reader.ReadInt32();
    }
}
