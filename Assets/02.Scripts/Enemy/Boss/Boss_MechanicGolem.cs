using UnityEngine;

[RequireComponent(typeof(BossAIManager))]
public class Boss_MechanicGolem : AEnemy
{
    public Collider WeaponCollider;
    private int _baseAttackCount = 0;

    protected override void Start()
    {
        base.Start();
    }

    public override void Init()
    {
        base.Init();
        _stateMachine.ChangeState(new BossIdleState());
    }

    public override void TakeDamage(Damage damage)
    {

    }

    public override void Attack()
    {
        WeaponCollider.enabled = true;
        EnemyRotation.IsFound = false;
    }

    public void OnBaseAttackEnd()
    {
        WeaponCollider.enabled = false;
        _baseAttackCount++;
        if(_baseAttackCount >= 4)
        {
            _baseAttackCount = 0;
            OnAnimationEnd();
        }
    }

    public void SpecialAttack_01()
    {

    }

    public void SpecialAttack_02()
    {

    }

    public void SpecialAttack_03()
    {

    }

    public void SpecialAttack_04()
    {

    }

    public override void OnAnimationEnd()
    {
        base.OnAnimationEnd();
        ChangeState(BossAIManager.Instance.DecideNextState());
    }
}
