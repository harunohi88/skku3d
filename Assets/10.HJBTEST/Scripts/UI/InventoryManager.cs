using UnityEngine;

namespace Rito.InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private InventoryUI _runeInventoryUI;
        [SerializeField] private InventoryUI _equipInventoryUI;
        
        [SerializeField] private Inventory _runeInventory;
        [SerializeField] private Inventory _equipInventory;

        public static InventoryManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject); return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public InventoryUI GetRuneInventoryUI() => _runeInventoryUI;
        public InventoryUI GetEquipInventoryUI() => _equipInventoryUI;
        

        private void Start()
        {
            // 각 인벤토리 UI에 상대 인벤토리 참조 설정
            _runeInventoryUI.SetInventoryReference(_runeInventory);
            _runeInventoryUI.SetOtherInventoryReference(_equipInventory, _equipInventoryUI);
            
            _equipInventoryUI.SetInventoryReference(_equipInventory);
            _equipInventoryUI.SetOtherInventoryReference(_runeInventory, _runeInventoryUI);
        }

        
    }
}
