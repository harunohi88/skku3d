using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _upgradeAndShopPanel;
    [SerializeField] private GameObject _equipmentPanel;
    [SerializeField] private GameObject _popupBackgroundImage;
    [SerializeField] private GameObject _mapPanel;

    private void Start()
    {
        _playerManager = PlayerManager.Instance;
        _inventoryPanel.SetActive(false);
        _upgradeAndShopPanel.SetActive(false);
        _equipmentPanel.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance.IsStart == false) return;
        HandleGameplayInput();
    }

    private void HandleGameplayInput()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        _playerManager.Move(moveInput);
        _playerManager.Rotate(moveInput);

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

        // 룬 장착과 인벤토리 토글
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            AudioManager.Instance.PlayUIAudio(UIAudioType.Tab);
            ToggleInventoryAndEquip();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.Instance.PlayUIAudio(UIAudioType.Tab);
            if (_mapPanel.activeSelf) _mapPanel.SetActive(false);
            else
            {
                _upgradeAndShopPanel.SetActive(false);
                _equipmentPanel.SetActive(false);

                _mapPanel.SetActive(true);
            }
        }

        // 업그레이드 상점과 인벤토리 토글
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (_equipmentPanel.activeSelf && _inventoryPanel.activeSelf)
            {
                _equipmentPanel.SetActive(false);
                _upgradeAndShopPanel.SetActive(true);
            }
            else
            {
                ToggleUpgradeAndInventory();
            }
        }
    }

    private void ToggleInventoryAndEquip()
    {
        bool nextState = !_inventoryPanel.activeSelf || !_equipmentPanel.activeSelf;
        _inventoryPanel.SetActive(nextState);
        _equipmentPanel.SetActive(nextState);
        _upgradeAndShopPanel.SetActive(false);

        InventoryManager.Instance.ToolTip.Hide();
        UpdateBackgroundImage();
    }

    private void ToggleUpgradeAndInventory()
    {
        bool nextState = !_upgradeAndShopPanel.activeSelf || !_inventoryPanel.activeSelf;
        _upgradeAndShopPanel.SetActive(nextState);
        _inventoryPanel.SetActive(nextState);
        _equipmentPanel.SetActive(false);
        UpdateBackgroundImage();
    }

    private void UpdateBackgroundImage()
    {
        bool anyPanelActive = _inventoryPanel.activeSelf || _equipmentPanel.activeSelf || _upgradeAndShopPanel.activeSelf;
        _popupBackgroundImage.SetActive(anyPanelActive);
    }
}
