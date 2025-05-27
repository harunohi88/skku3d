using UnityEngine;

public struct RuneExecuteContext
{
    public Player Player;
    public Damage Damage;
    public ISkill Skill;
    public AEnemy TargetEnemy;
    public float DistanceToTarget;
    public float TargetHelthPercentage;
    public bool IsKill;
}
