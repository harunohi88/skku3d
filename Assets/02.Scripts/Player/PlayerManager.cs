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
    [SerializeField] public PlayerSkill PlayerSkill;

    private Dictionary<EPlayerAction, HashSet<EPlayerState>> _actionStateMap = new()
    {
        { EPlayerAction.Attack, new HashSet<EPlayerState> { EPlayerState.None, EPlayerState.Attack, EPlayerState.Skill } },
        { EPlayerAction.Skill,  new HashSet<EPlayerState> { EPlayerState.None, EPlayerState.Attack, EPlayerState.Skill, EPlayerState.Targeting } },
        { EPlayerAction.Roll,   new HashSet<EPlayerState> { EPlayerState.None, EPlayerState.Attack, EPlayerState.Skill, EPlayerState.Targeting } },
        { EPlayerAction.Move,   new HashSet<EPlayerState> { EPlayerState.None, EPlayerState.Hit, EPlayerState.Targeting } },
    };

    private void Awake()
    {
        PlayerMove = Player.gameObject.GetComponent<PlayerMove>();
        PlayerAttack = Player.gameObject.GetComponent<PlayerAttack>();
        PlayerSkill = Player.gameObject.GetComponent<PlayerSkill>();
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

        if (PlayerState == EPlayerState.Targeting || PlayerState == EPlayerState.Skill)
        {
            PlayerSkill.Cancel();
        }
        PlayerMove.Roll(direction);
    }

    public void MouseInputLeft()
    {
        if (PlayerState == EPlayerState.Targeting)
        {
            PlayerSkill.CurrentSkill.Execute();
        }
        else
        {
            Attack();
        }
    }

    public void MouseInputRight()
    {
        if (PlayerState == EPlayerState.Targeting)
        {
            PlayerSkill.Cancel();
        }
        else
        {
            UseSkill(0);
        }
    }

    public void Attack()
    {
        if (!CanPerform(EPlayerAction.Attack)) return;

        if (PlayerState == EPlayerState.Attack)
        {
            PlayerAttack.InputQueued = true;
            return;
        }
        PlayerAttack.Attack();
    }

    public void UseSkill(int skillIndex)
    {
        if (!CanPerform(EPlayerAction.Skill)) return;

        PlayerSkill.UseSkill(skillIndex); // PlayerSKill.Skill 내부에서 PlayerState 변경
    }
}
