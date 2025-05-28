using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyRotation))]
public abstract class AEnemy : MonoBehaviour, IDamageable
{
    public float MaxHealth;
    public float Health;
    public float Damage;
    public float MoveSpeed = 3.5f;

    public EnemyType Type;

    public float TraceDistance;
    public float AttackDistance;
    public float AttackOutDistance;
    public float AttackCooltime;
    public float DamagedTime;
    public float DeathTime;

    public EnemySpawner ThisSpawner;

    public LayerMask LayerMask;

    public NavMeshAgent Agent;
    protected CharacterController _characterController;
    protected Animator _animator;

    protected StateMachine<AEnemy> _stateMachine;
    public IState<AEnemy> CurrentState => _stateMachine.CurrentState;

    public GameObject AttackPosition;
    public GameObject SkillObject;

    public EnemyRotation EnemyRotation;
    public EnemyHitEffect EnemyHitEffect;

    public Action OnStatChanged;

    protected virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        EnemyRotation = GetComponent<EnemyRotation>();
        EnemyHitEffect = GetComponentInChildren<EnemyHitEffect>();
        Agent.speed = MoveSpeed;
    }

    public virtual void Init(EnemySpawner spawner)
    {
        ThisSpawner = spawner;

        if (TimeManager.Instance.DifficultyMultiplier != null)
        {
            MaxHealth = (int)(MaxHealth * TimeManager.Instance.DifficultyMultiplier.EnemyHealthMultiplier);
            Damage = (int)(Damage * TimeManager.Instance.DifficultyMultiplier.EnemyDamageMultiplier);
        }

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
        if (_stateMachine.CurrentState is DieState) return;
        Health -= damage.Value;
        OnStatChanged?.Invoke();
        // 맞았을때 이펙트

        if (Health <= 0)
        {
            ChangeState(new DieState());
            return;
        }

        ChangeState(new DamagedState());
    }

    public abstract void Attack();

    public virtual void OnAnimationEnd()
    {
        EnemyRotation.IsFound = true;
    }

    public void SetAnimationTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }

    public bool IsPlayingAnimation(string animationName)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
    }
}
