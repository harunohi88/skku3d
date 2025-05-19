using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerMove _playerMove;

    private void Start()
    {
        _playerMove = PlayerManager.Instance.Player.gameObject.GetComponent<PlayerMove>();
    }

    private void Update()
    {
        HandleGameplayInput();
    }

    public void HandleGameplayInput()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _playerMove.Move(moveInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerMove.Roll();
        }
    }
}
