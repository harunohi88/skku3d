using UnityEngine;

[RequireComponent(typeof(Boss2AIManager))]
public class Boss_Ferex : AEnemy, IBoss2PatternHandler
{
    // 속성 정의
    public Collider WeaponCollider;
    private int _baseAttackCount = 0;


    protected void Start()
    {
        Debug.Log("임시 코드");
        Init(null);
    }

    public override void Init(EnemySpawner spawner)
    {
        base.Init(spawner);
        _stateMachine.ChangeState(new Boss2IdleState());
        //BossUIManager.Instance.SetBossUI("Ferex", MaxHealth); ///// HealthBar 추가한 코드
    }

    public override void TakeDamage(Damage damage)
    {
        if (_stateMachine.CurrentState is Boss2DieState) return;
        Health -= damage.Value;
        //BossUIManager.Instance.UPdateHealth(Health); ///// HealthBar 추가한 코드

        // 맞았을때 이펙트

        if (Health <= 0)
        {
            ChangeState(new Boss2DieState());
            return;
        }
        Debug.Log("맞음");
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
        if (_baseAttackCount >= 2)
        {
            _baseAttackCount = 0;
            BossAIManager.Instance.SetLastFinishedTime(0, Time.time); // 쿨타임 관리
            OnAnimationEnd();
        }
    }

    public void Boss2SpecialAttack_01()
    {
    }

    public void OnBoss2SpecialAttack01End()
    {

    }

    public void Boss2SpecialAttack_02()
    {


    }

    public void OnBos22SpecialAttack02End()
    {

    }

    public void Boss2SpecialAttack_03()
    {


    }

    public void OnBoss2SpecialAttack03End()
    {

    }

    public override void OnAnimationEnd()
    {
        base.OnAnimationEnd();
        ChangeState(new Boss2TraceState());
    }
}
