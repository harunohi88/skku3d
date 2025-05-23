// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
public enum RuneType
{
    ///<summary>정적</summary>
    Static = 0,
    ///<summary>동적</summary>
    Dynamic = 1,
}

public enum RuneDataType
{
    ///<summary>회복</summary>
    Recovery = 0,
    ///<summary>공격</summary>
    Damage = 1,
    ///<summary>쿨타임</summary>
    Cooltime = 2,
    ///<summary>이동속도</summary>
    MoveSpeed = 3,
    ///<summary>공격속도</summary>
    AttackSpeed = 4,
    ///<summary>치명타 피해</summary>
    CriticalDamage = 5,
    ///<summary>범위</summary>
    Range = 6,
    ///<summary>투사체</summary>
    Projectile = 7,
    ///<summary>체력</summary>
    Health = 8,
    ///<summary>무적</summary>
    Invincible = 9,
    ///<summary>적</summary>
    Enemy = 10,
    ///<summary>치명타 확률</summary>
    CriticalChance = 11,
}

public enum RuneTriggerType
{
    ///<summary>일반</summary>
    General = 0,
    ///<summary>스킬 사용 시</summary>
    Skill = 1,
    ///<summary>시간에 따라</summary>
    Time = 2,
    ///<summary>적 체력</summary>
    EnemyHealth = 3,
    ///<summary>거리</summary>
    Distance = 4,
    ///<summary>적 처치</summary>
    Kill = 5,
    ///<summary>데미지</summary>
    Damage = 6,
    ///<summary>치명타</summary>
    Critical = 7,
    ///<summary>보스</summary>
    Boss = 8,
    ///<summary>거리 안</summary>
    InDistance = 9,
    ///<summary>거리 밖</summary>
    OutDistance = 10,
    ///<summary>죽음</summary>
    Death = 11,
}

public enum EnemyAudioType
{
    ///<summary>공격</summary>
    Attack = 0,
    ///<summary>맞음</summary>
    Hit = 1,
    ///<summary>죽음</summary>
    Die = 2,
}

