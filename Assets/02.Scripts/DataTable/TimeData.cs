// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class TimeData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>현재 시간(초)</summary>
    public readonly int Time;

    ///<summary>난이도</summary>
    public readonly int DifficultyNum;

    ///<summary>난이도 설명</summary>
    public readonly string DifficultyText;

    ///<summary>몹 수 배수</summary>
    public readonly float EnemyCountMultiplier;

    ///<summary>몹 체력 배수</summary>
    public readonly float EnemyHealthMultiplier;

    ///<summary>몹 데미지 배수</summary>
    public readonly float EnemyDamageMultiplier;

    ///<summary>엘리트 스폰 확률</summary>
    public readonly float EliteSpawnRate;

    public TimeData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        Time = reader.ReadInt32();
        DifficultyNum = reader.ReadInt32();
        int difficultytext = reader.ReadInt32();
        DifficultyText = Encoding.UTF8.GetString(reader.ReadBytes(difficultytext));
        EnemyCountMultiplier = reader.ReadSingle();
        EnemyHealthMultiplier = reader.ReadSingle();
        EnemyDamageMultiplier = reader.ReadSingle();
        EliteSpawnRate = reader.ReadSingle();
    }
}
