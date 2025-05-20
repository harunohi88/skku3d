using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerManager : BehaviourSingleton<PlayerManager>
{
    [SerializeField] public Player Player;
    [SerializeField] public EPlayerState PlayerState;
    [SerializeField] public PlayerMove PlayerMove;
    [SerializeField] public PlayerAttack PlayerAttack;

    private Dictionary<EPlayerAction, HashSet<EPlayerState>> _actionStateMap = new()
    {
        { EPlayerAction.Attack, new HashSet<EPlayerState> { EPlayerState.None, EPlayerState.Attack, EPlayerState.Skill } },
        { EPlayerAction.Skill,  new HashSet<EPlayerState> { EPlayerState.None, EPlayerState.Attack, EPlayerState.Skill } },
        { EPlayerAction.Roll,   new HashSet<EPlayerState> { EPlayerState.None, EPlayerState.Attack, EPlayerState.Skill } },
        { EPlayerAction.Move,   new HashSet<EPlayerState> { EPlayerState.None, EPlayerState.Hit } },
    };

    private void Awake()
    {
        PlayerMove = Player.gameObject.GetComponent<PlayerMove>();
        PlayerAttack = Player.gameObject.GetComponent<PlayerAttack>();
        PlayerState = EPlayerState.None;
    }

    private bool CanPerform(EPlayerAction action)
    {
        return _actionStateMap[action].Contains(PlayerState);
    }

    public void Move(Vector2 inputDirection)
    {
        if (!CanPerform(EPlayerAction.Move)) return;
        PlayerMove.Move(inputDirection);
    }

    public void Roll(Vector2 direction)
    {
        if (!CanPerform(EPlayerAction.Roll)) return;
        PlayerMove.Roll(direction);
    }

    public void Attack()
    {
        if (!CanPerform(EPlayerAction.Attack)) return;

        if (PlayerState == EPlayerState.Attack)
        {
            PlayerAttack.InputQueued = true;
            return;
        }

        if (PlayerState == EPlayerState.Skill)
        {
            // PlayerSkill.ExecuteSkill();
        }
        PlayerAttack.Attack();
    }

    public void Skill(int skillIndex)
    {
        if (!CanPerform(EPlayerAction.Skill)) return;

        // 스킬 시전중 우클릭 입력시 처리할 로직이 여기 들어가야됨


        // PlayerSkill.Skill(skillIndex); // PlayerSKill.Skill 내부에서 PlayerState 변경
    }
}
