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

    ///<summary>적 타입</summary>
    public readonly EnemyType EnemyType;

    public ShopTableData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        Stage = reader.ReadInt32();
        EnemyType = (EnemyType)reader.ReadInt32();
    }
}
