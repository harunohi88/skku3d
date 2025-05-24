using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicInventory : BaseInventory
{
    [SerializeField] private EquipInventory equipInventory;

    public override bool AddItem(ARune rune, int quantity = 1)
    {
        // Try to find existing stack of the same rune
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].Rune.TID == rune.TID)
            {
                items[i].AddQuantity(quantity);
                UpdateSlot(i);
                return true;
            }
        }

        // Find empty slot
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = new InventoryItem(rune, quantity);
                UpdateSlot(i);
                SortInventory();
                return true;
            }
        }

        return false; // Inventory is full
    }

    private void SortInventory()
    {
        // Remove null items
        items.RemoveAll(item => item == null);
        
        // Sort by TID
        items = items.OrderBy(item => item.Rune.TID).ToList();
        
        // Fill remaining slots with null
        while (items.Count < slotCount)
        {
            items.Add(null);
        }

        // Update all slots
        for (int i = 0; i < slots.Count; i++)
        {
            UpdateSlot(i);
        }
    }

    public bool MoveItemToEquip(int fromSlot, int toSlot)
    {
        Debug.Log($"BasicInventory.MoveItemToEquip - From Slot: {fromSlot}, To Slot: {toSlot}");
        
        if (fromSlot < 0 || fromSlot >= items.Count || equipInventory == null)
        {
            Debug.Log($"BasicInventory.MoveItemToEquip - Invalid slot or equip inventory not set");
            return false;
        }

        // Move one item to equip inventory
        if (items[fromSlot] != null && items[fromSlot].Quantity > 0)
        {
            Debug.Log($"BasicInventory.MoveItemToEquip - Attempting to move item to equip inventory");
            // Try to add to equip inventory at specified slot
            if (equipInventory.AddItemToSlot(items[fromSlot].Rune, toSlot, 1))
            {
                Debug.Log($"BasicInventory.MoveItemToEquip - Successfully added to equip inventory");
                // Remove one item from source
                items[fromSlot].RemoveQuantity(1);
                if (items[fromSlot].Quantity <= 0)
                {
                    items[fromSlot] = null;
                }
                UpdateSlot(fromSlot);
                return true;
            }
            Debug.Log($"BasicInventory.MoveItemToEquip - Failed to add to equip inventory");
        }
        return false;
    }

    public override bool MoveItem(int fromSlot, int toSlot)
    {
        Debug.Log($"BasicInventory.MoveItem - From Slot: {fromSlot}, To Slot: {toSlot}");
        
        if (fromSlot < 0 || fromSlot >= items.Count || toSlot < 0 || toSlot >= items.Count)
        {
            Debug.Log($"BasicInventory.MoveItem - Invalid slot indices");
            return false;
        }

        // If both slots have items and they're the same type, stack them
        if (items[fromSlot] != null && items[toSlot] != null && 
            items[fromSlot].Rune.TID == items[toSlot].Rune.TID)
        {
            Debug.Log($"BasicInventory.MoveItem - Stacking items");
            items[toSlot].AddQuantity(items[fromSlot].Quantity);
            items[fromSlot] = null;
        }
        else
        {
            Debug.Log($"BasicInventory.MoveItem - Swapping items");
            // Regular swap
            InventoryItem temp = items[toSlot];
            items[toSlot] = items[fromSlot];
            items[fromSlot] = temp;
        }

        UpdateSlot(fromSlot);
        UpdateSlot(toSlot);
        SortInventory();
        return true;
    }
} 