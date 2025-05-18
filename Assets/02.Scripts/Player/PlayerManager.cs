using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : BehaviourSingleton<PlayerManager>
{
    [SerializeField] private PlayerMove _playerMove;

    private void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        HandleGameplayInput();
    }

    public void HandleGameplayInput()
    {
        // 이동
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _playerMove.Move(moveInput);

        // 구르기
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerMove.Roll();
        }
    }
}
