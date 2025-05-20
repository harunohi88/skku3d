using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    // 각각의 스킬이 언제 Activate 될건지. 입력이나 조건에 따른 해당 스킬의 Activate 실행.
    // 애니메이션 실행

    public GameObject Model;
    [SerializeField] private List<MonoBehaviour> skillComponents;
    private List<ISkill> skills = new List<ISkill>();
    public bool Istargeting = false;
    public int TargetingSlot;

    private Animator _animator;

    private void Awake()
    {
        _animator = Model.GetComponent<Animator>();
    }

    // 캐스트
    private void Start()
    {
        foreach (var component in skillComponents)
        {
            if (component is ISkill skill)
            {
                skills.Add(skill);
            }
        }
    }

    public void UseSkill(int slot)
    {
        if (slot < 0 || slot >= skills.Count) return;
        skills[slot].Execute();
        PlayerManager.Instance.PlayerState = EPlayerState.Skill;
    }
}
