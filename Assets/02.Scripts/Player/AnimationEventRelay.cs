using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerSkill _playerSkill;

    private void Awake()
    {
        _playerAttack = GetComponentInParent<PlayerAttack>();
        _playerSkill = GetComponentInParent<PlayerSkill>();
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

    public void OnSkillAnimationEffect()
    {
        _playerSkill.CurrentSkill?.OnSkillAnimationEffect();
    }
    
    public void OnSkillAnimationEnd()
    {
        _playerSkill.CurrentSkill?.OnSkillAnimationEnd();
    }

    public void PlayParticle(int index)
    {
        PlayerEffects.Instance.PlayParticle(index);
    }
}
