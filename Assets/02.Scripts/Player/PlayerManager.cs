using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += InitPlayer;
    }

    private void InitPlayer(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex < 1) return;
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
            if (Player == null) return;

            PlayerStat = Player.gameObject.GetComponent<PlayerStat>();
            PlayerMove = Player.gameObject.GetComponent<PlayerMove>();
            PlayerRotate = Player.gameObject.GetComponent<PlayerRotate>();
            PlayerAttack = Player.gameObject.GetComponent<PlayerAttack>();
            PlayerSkill = Player.gameObject.GetComponent<PlayerSkill>();
            PlayerLevel = Player.gameObject.GetComponent<PlayerLevel>();
            PlayerLevel = Player.gameObject.GetComponent<PlayerLevel>();
            PlayerState = EPlayerState.None;
        }

        GameObject spawnpoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        if(spawnpoint != null)
        {
            CharacterController controller =  Player.GetComponent<CharacterController>();
            controller.enabled = false;
            Player.transform.position = spawnpoint.transform.position;
            Player.transform.rotation = spawnpoint.transform.rotation;
            controller.enabled = true;
        }
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
