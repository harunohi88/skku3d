using Rito.InventorySystem;
using UnityEngine;

public class InvenDataReadTest : MonoBehaviour
{
    public Inventory EquipInventory;

    void Start()
    {
        // 슬롯이 업데이트 될 때마다 정보실행 되는 이벤트
        EquipInventory.UpdateSlotEvent += ItemLog;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var itemData = EquipInventory.GetItemData(0);
            Debug.Log(itemData != null ? $"{itemData.Name} {itemData.Tooltip}" : "빈 슬롯");

            var itemData2 = EquipInventory.GetItemData(1);
            if(itemData2 != null)
            {
                Debug.Log(itemData2.Name + " " + itemData2.Tooltip);
            }
            else
            {
                Debug.Log("빈 슬롯");
            }
            var itemData3 = EquipInventory.GetItemData(2);
            if(itemData3 != null)
            {
                Debug.Log(itemData3.Name + " " + itemData3.Tooltip);
            }
            else
            {
                Debug.Log("빈 슬롯");
            }
        }
    }

    public void ItemLog()
    {
        var itemData = EquipInventory.GetItemData(0);
            Debug.Log(itemData != null ? $"{itemData.Name} {itemData.Tooltip}" : "빈 슬롯");

            var itemData2 = EquipInventory.GetItemData(1);
            if(itemData2 != null)
            {
                Debug.Log(itemData2.Name + " " + itemData2.Tooltip);
            }
            else
            {
                Debug.Log("빈 슬롯");
            }
            var itemData3 = EquipInventory.GetItemData(2);
            if(itemData3 != null)
            {
                Debug.Log(itemData3.Name + " " + itemData3.Tooltip);
            }
            else
            {
                Debug.Log("빈 슬롯");
            }
    }
}
