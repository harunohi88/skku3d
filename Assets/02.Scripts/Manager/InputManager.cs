using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InputManager : BehaviourSingleton<InputManager>
{
    public PlayerManager _playerManager;
    public GameObject _inventoryPanel;
    public GameObject _upgradeAndShopPanel;
    public GameObject _equipmentPanel;
    public GameObject _popupBackgroundImage;
    public GameObject _mapPanel;
    public GameObject _optionPanel;

    public bool TurnOff;

    private void Start()
    {
        _playerManager = PlayerManager.Instance;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += InitScene;
    }

    private void InitScene(Scene scene, LoadSceneMode mode)
    {
        if (_inventoryPanel == null)
        {
            _inventoryPanel = GameObject.FindGameObjectWithTag("InventoryPanel");
            _inventoryPanel?.SetActive(false);

            _upgradeAndShopPanel = GameObject.FindGameObjectWithTag("UpgradeAndShopPanel");
            _upgradeAndShopPanel?.SetActive(false);

            _equipmentPanel = GameObject.FindGameObjectWithTag("EquipmentPanel");
            _equipmentPanel?.SetActive(false);

            _popupBackgroundImage = GameObject.FindGameObjectWithTag("PopUpBG");
            _popupBackgroundImage?.SetActive(false);

            _optionPanel = GameObject.FindGameObjectWithTag("OptionPanel");
            _optionPanel?.SetActive(false);
        }

        _mapPanel = GameObject.FindGameObjectWithTag("MapPanel");
        _mapPanel?.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance.IsStart == false) return;
        if(_playerManager.Player) HandleGameplayInput();
    }

    private void HandleGameplayInput()
    {
        if (TurnOff) return;
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        _playerManager.Move(moveInput);
        _playerManager.Rotate(moveInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerManager.Roll(moveInput);
        }

        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
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
        }

        // 룬 장착과 인벤토리 토글
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_inventoryPanel == null) return;
            AudioManager.Instance.PlayUIAudio(UIAudioType.Tab);
            ToggleInventoryAndEquip();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (_mapPanel == null) return;
            AudioManager.Instance.PlayUIAudio(UIAudioType.Tab);
            if (_mapPanel.activeSelf) _mapPanel.SetActive(false);
            else
            {
                _upgradeAndShopPanel.SetActive(false);
                _equipmentPanel.SetActive(false);
                _optionPanel.SetActive(false);

                _mapPanel.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_optionPanel == null) return;
            AudioManager.Instance.PlayUIAudio(UIAudioType.Tab);
            if (_optionPanel.activeSelf) _optionPanel.SetActive(false);
            else
            {
                _upgradeAndShopPanel.SetActive(false);
                _equipmentPanel.SetActive(false);
                _inventoryPanel.SetActive(false);

                _optionPanel.SetActive(true);
            }
        }
    }

    private void ToggleInventoryAndEquip()
    {
        bool nextState = !_inventoryPanel.activeSelf || !_equipmentPanel.activeSelf;
        _inventoryPanel.SetActive(nextState);
        _equipmentPanel.SetActive(nextState);
        _upgradeAndShopPanel.SetActive(false);
        _optionPanel.SetActive(false);

        InventoryManager.Instance.ToolTip.Hide();
        UpdateBackgroundImage();
    }

    public void ToggleUpgradeAndInventory()
    {
        bool nextState = !_upgradeAndShopPanel.activeSelf || !_inventoryPanel.activeSelf;
        _upgradeAndShopPanel.SetActive(nextState);
        _inventoryPanel.SetActive(nextState);
        _equipmentPanel.SetActive(false);
        _optionPanel.SetActive(false);

        UpdateBackgroundImage();
    }

    public void UpdateBackgroundImage()
    {
        bool anyPanelActive = _inventoryPanel.activeSelf || _equipmentPanel.activeSelf || _upgradeAndShopPanel.activeSelf;
        _popupBackgroundImage.SetActive(anyPanelActive);
    }
}
