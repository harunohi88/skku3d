using UnityEngine;

namespace Rito.InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private InventoryUI playerInventoryUI;
        [SerializeField] private InventoryUI storageInventoryUI;
        
        [SerializeField] private Inventory playerInventory;
        [SerializeField] private Inventory storageInventory;

        private void Start()
        {
            // 각 인벤토리 UI에 상대 인벤토리 참조 설정
            playerInventoryUI.SetInventoryReference(playerInventory);
            playerInventoryUI.SetOtherInventoryReference(storageInventory, storageInventoryUI);
            
            storageInventoryUI.SetInventoryReference(storageInventory);
            storageInventoryUI.SetOtherInventoryReference(playerInventory, playerInventoryUI);
        }
    }
}
