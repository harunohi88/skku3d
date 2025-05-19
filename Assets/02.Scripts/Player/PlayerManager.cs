using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : BehaviourSingleton<PlayerManager>
{
    [SerializeField] public Player Player;
    [SerializeField] public EPlayerState PlayerStatus;

    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private PlayerAttack _playerAttack;

    private void Awake()
    {
        _playerMove = Player.gameObject.GetComponent<PlayerMove>();
        _playerAttack = Player.gameObject.GetComponent<PlayerAttack>();
        PlayerStatus = EPlayerState.None;
    }

    public void Move(Vector2 inputDirection)
    {
        _playerMove.Move(inputDirection);
    }

    public void Roll()
    {
        _playerMove.Roll();
    }
}
