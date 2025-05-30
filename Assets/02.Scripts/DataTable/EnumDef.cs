// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
public enum RuneType
{
    ///<summary>정적</summary>
    Static = 0,
    ///<summary>동적</summary>
    Dynamic = 1,
}

public enum EffectTimingType
{
    ///<summary>공격 전</summary>
    BeforeAttack = 0,
    ///<summary>공격 후</summary>
    AfterAttack = 1,
    ///<summary>시전당 1회</summary>
    OncePerAttack = 2,
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
    ///<summary>난도질의 룬 (동적 룬)</summary>
    SlaughterRuneEffect = 12,
    ///<summary>난사의 룬 (동적 룬)</summary>
    ArrowRuneEffect = 13,
    ///<summary>불꽃의 룬 (동적 룬)</summary>
    FlameRuneEffect = 14,
    ///<summary>폭렬의 룬 (동적 룬)</summary>
    ExplosionRuneEffect = 15,
    ///<summary>부활의 룬 (동적 룬)</summary>
    RevivalRuneEffect = 16,
    ///<summary>운명의 룬 (동적 룬)</summary>
    FateRuneEffect = 17,
    ///<summary>전류의 룬 (동적 룬)</summary>
    ElectricRuneEffect = 18,
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
    ///<summary>장착 시</summary>
    OnEquipTrigger = 8,
    ///<summary>죽을 시</summary>
    OnDeathTrigger = 9,
    ///<summary>공격 전</summary>
    OnBeforeAttackTrigger = 10,
    ///<summary>공격 후</summary>
    OnAfterAttactTrigger = 11,
    ///<summary>스킬 시전당 1회</summary>
    OnOncePerAttackTrigger = 12,
}

public enum EnemyType
{
    ///<summary>기본</summary>
    Basic = 0,
    ///<summary>엘리트</summary>
    Elite = 1,
    ///<summary>보스</summary>
    Boss = 2,
}

public enum EStatType
{
    ///<summary>데미지</summary>
    AttackPower = 0,
    ///<summary>쿨타임 감소</summary>
    CooldownReduction = 1,
    ///<summary>치명타 확률</summary>
    CriticalHitChance = 2,
    ///<summary>치명타 피해</summary>
    CriticalHitDamage = 3,
    ///<summary>경험치 획득량</summary>
    ExperienceGain = 4,
    ///<summary>최대 체력</summary>
    MaxHealth = 5,
    ///<summary>최대 스태미나</summary>
    MaxStamina = 6,
    ///<summary>이동 속도</summary>
    MoveSpeed = 7,
    ///<summary>투사체 증가량</summary>
    ProjectileCountGain = 8,
}

public enum ESkillType
{
    ///<summary>기본공격</summary>
    Skill0 = 0,
    ///<summary>스킬1</summary>
    Skill1 = 1,
    ///<summary>스킬2</summary>
    Skill2 = 2,
    ///<summary>스킬3</summary>
    Skill3 = 3,
    ///<summary>구르기</summary>
    Roll = 4,
}

public enum CurrencyUseType
{
    ///<summary>스킬 업그레이드</summary>
    SkillUpgrade = 0,
    ///<summary>룬 티어1 구매</summary>
    BuyRuneTier1 = 1,
    ///<summary>룬 티어2 구매</summary>
    BuyRuneTier2 = 2,
    ///<summary>룬 티어3 구매</summary>
    BuyRuneTier3 = 3,
    ///<summary>룬 리롤</summary>
    ReRollRune = 4,
}

public enum SkillUpgradeType
{
    ///<summary>데미지 배수</summary>
    DamageMultiplier = 0,
    ///<summary>쿨타임 감소</summary>
    CooldownReduction = 1,
    ///<summary>범위 증가</summary>
    Range = 2,
    ///<summary>스태미나 감소</summary>
    StaminaReduction = 3,
    ///<summary>없음</summary>
    None = 4,
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

