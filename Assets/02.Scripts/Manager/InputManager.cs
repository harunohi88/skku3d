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
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        _playerManager.Move(moveInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerManager.Roll(moveInput);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _playerManager.Attack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _playerManager.Skill(0);
        }
    }
}
