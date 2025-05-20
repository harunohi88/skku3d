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
        PlayerManager.Instance.PlayerState = EPlayerState.Attack;
        _animator.SetTrigger(AttackTriggerList[_currentAttackIndex]);
    }

    public void Cancle()
    {
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
            Cancle();
        }
    }

    public void OnAttackLoopEnd()
    {
        Cancle();
    }
}
