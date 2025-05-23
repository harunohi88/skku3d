using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private InventoryManager _inventoryManager;

    private void Start()
    {
        _playerManager = PlayerManager.Instance;
        _inventoryManager = InventoryManager.Instance;
    }

    private void Update()
    {
        HandleGameplayInput();
    }

    private void HandleGameplayInput()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        _playerManager.Move(moveInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerManager.Roll(moveInput);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _playerManager.MouseInputLeft();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _playerManager.MouseInputRight();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _playerManager.UseSkill(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _inventoryManager.ToggleRuneInventoryUI();
            _inventoryManager.ToggleEquipInventoryUI();
        }
    }
}
