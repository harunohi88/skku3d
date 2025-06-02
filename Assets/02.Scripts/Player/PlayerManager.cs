using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerManager : BehaviourSingleton<PlayerManager>
{
    [SerializeField] public Player Player;
    [SerializeField] public PlayerStat PlayerStat;
    [SerializeField] public PlayerMove PlayerMove;
    [SerializeField] public PlayerRotate PlayerRotate;
    [SerializeField] public PlayerAttack PlayerAttack;
    [SerializeField] public PlayerSkill PlayerSkill;
    [SerializeField] public PlayerLevel PlayerLevel;
    [SerializeField] public EPlayerState PlayerState;

    private Dictionary<EPlayerAction, HashSet<EPlayerState>> _actionStateMap = new()
    {
        { EPlayerAction.Attack, new HashSet<EPlayerState> { EPlayerState.None, EPlayerState.Attack, EPlayerState.Move } },
        { EPlayerAction.Skill,  new HashSet<EPlayerState> { EPlayerState.None, EPlayerState.Move, EPlayerState.Attack, EPlayerState.Skill, EPlayerState.Targeting } },
        { EPlayerAction.Roll,   new HashSet<EPlayerState> { EPlayerState.None, EPlayerState.Move, EPlayerState.Attack, EPlayerState.Skill, EPlayerState.Targeting } },
        { EPlayerAction.Move,   new HashSet<EPlayerState> { EPlayerState.None, EPlayerState.Move, EPlayerState.Attack, EPlayerState.Hit, EPlayerState.Targeting } },
        { EPlayerAction.Rotate, new HashSet<EPlayerState> { EPlayerState.None, EPlayerState.Move, EPlayerState.Targeting } },
    };

    private void Awake()
    {
        PlayerStat = Player.gameObject.GetComponent<PlayerStat>();
        PlayerMove = Player.gameObject.GetComponent<PlayerMove>();
        PlayerRotate = Player.gameObject.GetComponent<PlayerRotate>();
        PlayerAttack = Player.gameObject.GetComponent<PlayerAttack>();
        PlayerSkill = Player.gameObject.GetComponent<PlayerSkill>();
        PlayerLevel = Player.gameObject.GetComponent<PlayerLevel>();
        PlayerLevel = Player.gameObject.GetComponent<PlayerLevel>();
        PlayerState = EPlayerState.None;

        DontDestroyOnLoad(gameObject);
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
    
    public void Rotate(Vector2 inputDirection)
    {
        if (!CanPerform(EPlayerAction.Rotate)) return;

        if (PlayerAttack.IsAttacking) return;

        PlayerRotate.Rotate(inputDirection);
    }

    public void Roll(Vector2 direction)
    {
        if (!CanPerform(EPlayerAction.Roll)) return;

        if (PlayerSkill.IsTargeting || PlayerSkill.CurrentSkill != null)
        {
            PlayerRotate.CancelRotation();
            PlayerSkill.Cancel();
        }
        
        if (PlayerAttack.IsAttacking)
        {
            PlayerRotate.CancelRotation();
            PlayerAttack.Cancel();
        }
        PlayerMove.Roll(direction);
    }

    public void MouseInputLeft()
    {
        if (PlayerSkill.IsTargeting)
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
        if (PlayerSkill.IsTargeting)
        {
            PlayerSkill.Cancel();
        }
        else if (PlayerAttack.IsAttacking)
        {
            PlayerAttack.Cancel();
            UseSkill(0);
        }
        else
        {
            UseSkill(0);
        }
    }

    public void Attack()
    {
        if (!CanPerform(EPlayerAction.Attack)) return;

        if (PlayerAttack.IsAttacking)
        {
            PlayerAttack.InputQueued = true;
            return;
        }
        PlayerAttack.Attack();
    }

    public void UseSkill(int skillIndex)
    {
        if (!CanPerform(EPlayerAction.Skill)) return;

        if (PlayerAttack.IsAttacking)
        {
            PlayerRotate.CancelRotation();
            PlayerAttack.Cancel();
        }
        PlayerSkill.UseSkill(skillIndex); // PlayerSKill.Skill 내부에서 PlayerState 변경
    }
}
