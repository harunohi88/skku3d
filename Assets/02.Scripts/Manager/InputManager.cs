using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;

    private void Start()
    {
        _playerManager = PlayerManager.Instance;
    }

    private void Update()
    {
        HandleGameplayInput();
    }

    public void HandleGameplayInput()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _playerManager.Move(moveInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerManager.Roll();
        }
    }
}
