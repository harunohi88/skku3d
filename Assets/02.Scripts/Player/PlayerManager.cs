using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : BehaviourSingleton<PlayerManager>
{
    [SerializeField] private PlayerMove _playerMove;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
    }
}
