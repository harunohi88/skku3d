using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    private PlayerAttack _playerAttack;

    private void Awake()
    {
        _playerAttack = GetComponentInParent<PlayerAttack>();
    }

    public void OnAttackAnimationHit()
    {
        _playerAttack.OnAttackAnimationHit();
    }

    public void OnAttackAnimationEnd()
    {
        _playerAttack.OnAttackAnimationEnd();
    }

    public void OnAttackLoopEnd()
    {
        _playerAttack.OnAttackLoopEnd();
    }
}
