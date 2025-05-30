// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class CurrencyData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>재화 사용 타입</summary>
    public readonly CurrencyUseType CurrencyUseType;

    public CurrencyData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        CurrencyUseType = (CurrencyUseType)reader.ReadInt32();
    }
}
