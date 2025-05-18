using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerMove _playerMove;

    private void Start()
    {
        _playerMove = PlayerManager.Instance.gameObject.GetComponent<PlayerMove>();
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
