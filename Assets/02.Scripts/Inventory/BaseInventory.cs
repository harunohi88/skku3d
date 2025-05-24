using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseInventory : MonoBehaviour
{
    [SerializeField] protected int slotCount = 20;
    [SerializeField] protected GameObject slotPrefab;
    [SerializeField] protected Transform slotsParent;
    [SerializeField] protected bool autoCreateSlots = true;
    [SerializeField] public List<InventorySlot> manualSlots;
    
    protected List<InventoryItem> items;
    protected List<InventorySlot> slots;
    
    protected virtual void Awake()
    {
        items = new List<InventoryItem>();
        slots = new List<InventorySlot>();

        if (autoCreateSlots)
        {
            // Create slots automatically
            for (int i = 0; i < slotCount; i++)
            {
                GameObject slotObj = Instantiate(slotPrefab, slotsParent);
                InventorySlot slot = slotObj.GetComponent<InventorySlot>();
                slot.Initialize(this, i);
                slots.Add(slot);
                items.Add(null);
            }
        }
        else
        {
            // Use manually placed slots
            for (int i = 0; i < manualSlots.Count; i++)
            {
                manualSlots[i].Initialize(this, i);
                slots.Add(manualSlots[i]);
                items.Add(null);
            }
            slotCount = manualSlots.Count;
        }

        UpdateAllSlots();
    }

    public virtual bool AddItem(ARune rune, int quantity = 1)
    {
        return false; // To be implemented by derived classes
    }

    public virtual bool RemoveItem(int slotIndex, int quantity = 1)
    {
        if (slotIndex < 0 || slotIndex >= items.Count || items[slotIndex] == null)
            return false;

        items[slotIndex].RemoveQuantity(quantity);
        if (items[slotIndex].IsEmpty())
        {
            items[slotIndex] = null;
        }
        UpdateSlot(slotIndex);
        return true;
    }

    public virtual bool MoveItem(int fromSlot, int toSlot)
    {
        if (fromSlot < 0 || fromSlot >= items.Count || toSlot < 0 || toSlot >= items.Count)
            return false;

        InventoryItem temp = items[toSlot];
        items[toSlot] = items[fromSlot];
        items[fromSlot] = temp;

        UpdateSlot(fromSlot);
        UpdateSlot(toSlot);
        return true;
    }

    public virtual void UpdateSlot(int index)
    {
        if (index >= 0 && index < slots.Count)
        {
            slots[index].UpdateSlot(items[index]);
        }
    }

    protected void UpdateAllSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].UpdateSlot(items[i]);
        }
    }

    public virtual void OnItemDoubleClick(int slotIndex)
    {
        // To be implemented by derived classes
    }

    public List<ARune> GetRuneList()
    {
        List<ARune> runeList = new List<ARune>();
        foreach (var item in items)
        {
            if (item != null && !item.IsEmpty())
            {
                runeList.Add(item.Rune);
            }
        }
        return runeList;
    }
} 