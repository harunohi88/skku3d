using UnityEngine;

[System.Serializable]
public class EnemyPatternData
{
    public float CoolTime;        // 패턴 쿨타임
    public float CastingTime;     // 캐스팅 시간
    public float LastFinishedTime; // 마지막 사용 시간
    public float Range;           // 기본 범위
    public float Width;           // 사각형의 가로 길이
    public float Radius;          // 반경 (원형 패턴용)
    public float Angle;           // 각도 (부채꼴 패턴용)
    public float Duration;        // 지속 시간
    public int MaxCount;          // 최대 사용 횟수
    public float Damage;          // 데미지
    public float InnerRange;      // 내부 범위 (도넛형 패턴용)
} 