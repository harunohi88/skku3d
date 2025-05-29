using UnityEngine;

[RequireComponent(typeof(BossAIManager))]
public class Boss_SpiritDemon : AEnemy, ISpecialAttackable
{
    private int _baseAttackCount = 0;
    private int _attackCount = 0;

    private void Start()
    {
        Init(null);
    }

    public override void Init(EnemySpawner spawner)
    {
        base.Init(spawner);
        _stateMachine.ChangeState(new Boss3IdleState());
        // BossUIManager.Instance.SetBossUI();  ///// HealthBar 추가한 코드
    }

    public override void TakeDamage(Damage damage)
    {
        if (_stateMachine.CurrentState is Boss3DieState) return;
        Health -= damage.Value;
       //  BossUIManager.Instance.UpdateHealth(Health);    ///// HealthBar 추가한 코드

       if (Health <= 0)
       {
            ChangeState(new Boss3DieState());
            return;
       }

       Debug.Log($"{gameObject.name} {damage.Value} 데미지 입음");
    }

    public override void Attack()
    {
        EnemyRotation.IsFound = false;
    }

    public void SpecialAttack_01()
    {
        throw new System.NotImplementedException();
    }

    public void OnSpecialAttack01End()
    {
        throw new System.NotImplementedException();
    }
    
    public void SpecialAttack_02()
    {
        throw new System.NotImplementedException();
    }

    public void OnSpecialAttack02End()
    {
        throw new System.NotImplementedException();
    }

    public void SpecialAttack_03()
    {
        throw new System.NotImplementedException();
    }

    public void OnSpecialAttack03End()
    {
        throw new System.NotImplementedException();
    }
    
    public void SpecialAttack_04()
    {
        throw new System.NotImplementedException();
    }

    public void OnSpecialAttack04End()
    {
        throw new System.NotImplementedException();
    }

    public override void OnAnimationEnd()
    {
        base.OnAnimationEnd();
        ChangeState(new Boss3TraceState());
    }
}
