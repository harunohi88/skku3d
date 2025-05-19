using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : BehaviourSingleton<PlayerManager>
{
    [SerializeField] public Player Player;

    [SerializeField] private PlayerMove _playerMove;

    private void Awake()
    {
        _playerMove = Player.gameObject.GetComponent<PlayerMove>();
    }
}
