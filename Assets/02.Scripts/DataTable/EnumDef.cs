// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
public enum RuneType
{
    ///<summary>정적</summary>
    Static = 0,
    ///<summary>동적</summary>
    Dynamic = 1,
}

public enum RuneEffectType
{
    ///<summary>없음</summary>
    None = 0,
    ///<summary>체력 회복</summary>
    RecoverEffect = 1,
    ///<summary>이동 속도 증가</summary>
    MoveSpeedBuffEffect = 2,
    ///<summary>피해 증가</summary>
    DamageBuffEffect = 3,
    ///<summary>치명타 확률 증가</summary>
    CriticalChanceBuffEffect = 4,
    ///<summary>치명타 피해 증가</summary>
    CriticalDamageBuffEffect = 5,
    ///<summary>최대 체력 증가</summary>
    HealthIncreaseEffect = 6,
    ///<summary>스킬 범위 증가</summary>
    RangeBuffEffect = 7,
    ///<summary>쿨타임 감소</summary>
    CooldownReductionEffect = 8,
    ///<summary>투사체 수 증가</summary>
    ProjectileCountEffect = 9,
    ///<summary>흡혈 (피해량 비례 회복)</summary>
    VampiricEffect = 10,
    ///<summary>체력 기반 피해 증가</summary>
    HPScalingDamageEffect = 11,
}

public enum StatModifierType
{
    ///<summary>없음</summary>
    None = 0,
    ///<summary>최대 체력 증가</summary>
    MaxHealthModifier = 1,
    ///<summary>이동 속도 증가</summary>
    MoveSpeedModifier = 2,
    ///<summary>투사체 수 증가</summary>
    ProjectileModifier = 3,
    ///<summary>치명타 확률 증가</summary>
    CriticalChanceModifier = 4,
    ///<summary>치명타 피해 증가</summary>
    CriticalDamageModifier = 5,
    ///<summary>스킬 범위 증가</summary>
    RangeModifier = 6,
}

public enum RuneTriggerType
{
    ///<summary>없음</summary>
    None = 0,
    ///<summary>스킬 사용 시</summary>
    OnSkillUseTrigger = 1,
    ///<summary>주기적 발동</summary>
    OnTimeElapsedTrigger = 2,
    ///<summary>적 체력 조건에 따라</summary>
    OnEnemyHealthConditionTrigger = 3,
    ///<summary>근접 여부</summary>
    OnDistanceCheckTrigger = 4,
    ///<summary>치명타 시</summary>
    OnCriticalHitTrigger = 5,
    ///<summary>적 타격 시</summary>
    OnEnemyDamagedTrigger = 6,
    ///<summary>보스 대상</summary>
    OnBossHitTrigger = 7,
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

