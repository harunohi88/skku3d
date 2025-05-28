using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseInventory : MonoBehaviour
{
    [SerializeField] protected int _slotCount = 20;
    [SerializeField] protected GameObject _slotPrefab;
    [SerializeField] protected Transform _slotsParent;
    [SerializeField] protected bool _autoCreateSlots = true;
    [SerializeField] public List<InventorySlot> ManualSlotsList;
    
    protected List<InventoryItem> _itemsList;
    protected List<InventorySlot> _slotsList;
    
    [SerializeField] protected Tooltip _tooltip;
    
    protected virtual void Awake()
    {
        _itemsList = new List<InventoryItem>();
        _slotsList = new List<InventorySlot>();

        if (_autoCreateSlots)
        {
            // 슬롯 자동 생성
            for (int i = 0; i < _slotCount; i++)
            {
                GameObject slotObj = Instantiate(_slotPrefab, _slotsParent);
                InventorySlot slot = slotObj.GetComponent<InventorySlot>();
                slot.Initialize(this, i, _tooltip);
                _slotsList.Add(slot);
                _itemsList.Add(null);
            }
        }
        else
        {
            // 수동으로 배치된 슬롯 사용
            for (int i = 0; i < ManualSlotsList.Count; i++)
            {
                ManualSlotsList[i].Initialize(this, i, _tooltip);
                _slotsList.Add(ManualSlotsList[i]);
                _itemsList.Add(null);
            }
            _slotCount = ManualSlotsList.Count;
        }

        UpdateAllSlots();
    }

    public virtual bool AddItem(Rune rune, int quantity = 1)
    {
        return false; // 파생 클래스에서 구현해야 함
    }

    public virtual bool RemoveItem(int slotIndex, int quantity = 1)
    {
        if (slotIndex < 0 || slotIndex >= _itemsList.Count || _itemsList[slotIndex] == null)
            return false;

        _itemsList[slotIndex].RemoveQuantity(quantity);
        if (_itemsList[slotIndex].IsEmpty())
        {
            _itemsList[slotIndex] = null;
        }
        UpdateSlot(slotIndex);
        return true;
    }

    public virtual bool MoveItem(int fromSlot, int toSlot)
    {
        if (fromSlot < 0 || fromSlot >= _itemsList.Count || toSlot < 0 || toSlot >= _itemsList.Count)
            return false;

        InventoryItem temp = _itemsList[toSlot];
        _itemsList[toSlot] = _itemsList[fromSlot];
        _itemsList[fromSlot] = temp;

        UpdateSlot(fromSlot);
        UpdateSlot(toSlot);
        return true;
    }

    public virtual void UpdateSlot(int index)
    {
        if (index >= 0 && index < _slotsList.Count)
        {
            _slotsList[index].UpdateSlot(_itemsList[index]);
        }
    }

    protected void UpdateAllSlots()
    {
        for (int i = 0; i < _slotsList.Count; i++)
        {
            _slotsList[i].UpdateSlot(_itemsList[i]);
        }
    }

    public virtual void OnItemDoubleClick(int slotIndex)
    {
        // 파생 클래스에서 구현해야 함
    }

    public List<Rune> GetRuneList()
    {
        List<Rune> runeList = new List<Rune>();
        foreach (var item in _itemsList)
        {
            if (item != null && !item.IsEmpty())
            {
                runeList.Add(item.Rune);
            }
        }
        return runeList;
    }
} 