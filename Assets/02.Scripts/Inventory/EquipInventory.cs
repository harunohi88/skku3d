using UnityEngine;

public class EquipInventory : BaseInventory
{
    [SerializeField] private BasicInventory basicInventory;

    protected override void Awake()
    {
        autoCreateSlots = false; // 기본적으로 수동 슬롯 배치 사용
        base.Awake();
    }

    public override bool AddItem(ARune rune, int quantity = 1)
    {
        // Find empty slot
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = new InventoryItem(rune, 1); // Always quantity 1 in equip inventory
                UpdateSlot(i);
                return true;
            }
        }

        return false; // No empty slots
    }

    public bool AddItemToSlot(ARune rune, int slotIndex, int quantity = 1)
    {
        if (slotIndex < 0 || slotIndex >= items.Count)
            return false;

        // Check if slot is empty
        if (items[slotIndex] == null)
        {
            items[slotIndex] = new InventoryItem(rune, 1); // Always quantity 1 in equip inventory
            UpdateSlot(slotIndex);
            return true;
        }

        return false; // Slot is not empty
    }

    public override bool MoveItem(int fromSlot, int toSlot)
    {
        if (fromSlot < 0 || fromSlot >= items.Count || toSlot < 0 || toSlot >= items.Count)
            return false;

        // Regular swap
        InventoryItem temp = items[toSlot];
        items[toSlot] = items[fromSlot];
        items[fromSlot] = temp;

        UpdateSlot(fromSlot);
        UpdateSlot(toSlot);
        return true;
    }

    public override void OnItemDoubleClick(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= items.Count || items[slotIndex] == null)
            return;

        // Move item back to basic inventory
        ARune rune = items[slotIndex].Rune;
        if (basicInventory.AddItem(rune, 1))
        {
            items[slotIndex] = null;
            UpdateSlot(slotIndex);
        }
    }

    /*
    // 모든 슬롯의 룬 정보를 배열로 반환
    public ARune[] GetEquippedRunes()
    {
        ARune[] equippedRunes = new ARune[items.Count];
        for (int i = 0; i < items.Count; i++)
        {
            equippedRunes[i] = items[i]?.Rune;
        }
        return equippedRunes;
    }*/

    // 특정 슬롯의 룬 정보를 반환
    public ARune GetRuneAtSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= items.Count)
            return null;
            
        return items[slotIndex]?.Rune;
    }
} 