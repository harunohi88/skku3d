using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    public GameObject Model;
    public List<string> AttackTriggerList;
    public bool InputQueued;

    private int _currentAttackIndex;

    private Animator _animator;

    private void Awake()
    {
        _animator = Model.GetComponent<Animator>();
    }

    public void Attack()
    {
        _animator.ResetTrigger("Attack1");
        _animator.ResetTrigger("Attack2");
        _animator.ResetTrigger("Attack3");
        PlayerManager.Instance.PlayerState = EPlayerState.Attack;
        _animator.SetTrigger(AttackTriggerList[_currentAttackIndex]);
    }

    public void Cancle()
    {
        Debug.Log("Cancle Attack");
        _animator.ResetTrigger("Attack1");
        _animator.ResetTrigger("Attack2");
        _animator.ResetTrigger("Attack3");
        _animator.SetTrigger("Idle");
        PlayerManager.Instance.PlayerState = EPlayerState.None;
        InputQueued = false;
        _currentAttackIndex = 0;
    }

    public void OnAttackAnimationHit()
    {
        // Handle attack hit logic here (e.g., deal damage to enemies)
    }

    public void OnAttackAnimationEnd()
    {
        if (InputQueued)
        {
            InputQueued = false;
            _currentAttackIndex = (_currentAttackIndex + 1) % AttackTriggerList.Count;
            Attack();
        }
        else
        {
            _animator.SetTrigger("Idle");
            Cancle();
        }
    }

    public void OnAttackLoopEnd()
    {
        _animator.SetTrigger("Idle");
        Cancle();
    }
}
