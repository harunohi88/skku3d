using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    public GameObject Model;
    public List<string> AttackTriggerList;

    private bool _isAttacking;
    private bool _inputQueued;
    private int _currentAttackIndex;

    private Animator _animator;

    private void Awake()
    {
        _animator = Model.GetComponent<Animator>();
    }
}
