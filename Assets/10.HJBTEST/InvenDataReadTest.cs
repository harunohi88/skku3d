using UnityEngine;
using System.Collections.Generic;

public class InvenDataReadTest : MonoBehaviour
{
    public Inventory EquipInventory;

    void Start()
    {
        // TODO: 지금은은 슬롯이 업데이트 될 때마다 정보실행 되는 이벤트에 구독
        // 나중에는 정보를 출력하는 것이 아니라 룬에 적용할 수 있도록 해야한다.
        // 테스트용이니까 참고해서 따로 만들어야 한다.
        EquipInventory.UpdateSlotEvent += ItemLog;
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
