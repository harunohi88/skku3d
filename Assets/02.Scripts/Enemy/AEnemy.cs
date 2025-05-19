using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyRotation))]
public abstract class AEnemy : MonoBehaviour, IDamageable
{
    public int MaxHealth;
    public int Health;
    public int Damage;

    public float TraceDistance;
    public float AttackDistance;
    public float AttackOutDistance;
    public float AttackCooltime;
    public float DamagedTime;
    public float DeathTime;

    public NavMeshAgent Agent;
    protected CharacterController _characterController;
    protected Animator _animator;

    protected StateMachine<AEnemy> _stateMachine;
    public IState<AEnemy> CurrentState => _stateMachine.CurrentState;

    public GameObject AttackPosition;
    public GameObject SkillObject;

    public EnemyRotation EnemyRotation;

    protected virtual void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        EnemyRotation = GetComponent<EnemyRotation>();

        Init();
    }

    public virtual void Init()
    {
        Health = MaxHealth;
        _stateMachine = new StateMachine<AEnemy>(this);
    }

    protected virtual void Update()
    {
        _stateMachine.Update();
    }

    public virtual void ChangeState(IState<AEnemy> newState)
    {
        _stateMachine.ChangeState(newState);
    }

    public virtual void TakeDamage(Damage damage)
    {
        // 죽음 상태면 return
        // if (_stateMachine.CurrentState is DieState) return;
        Health -= damage.Value;

        // 맞았을때 이펙트

        if(Health <= 0)
        {
            // ChangeState(new DieState());
        }

        // ChangeState(new DamageState());
    }

    public abstract void Attack();

    public void SetAnimationTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }

    public bool IsPlayingAnimation(string animationName)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
}
