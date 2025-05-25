using UnityEngine;

public class EquipInventory : BaseInventory
{
    [SerializeField] private BasicInventory _basicInventory;

    protected override void Awake()
    {
        _autoCreateSlots = false; // 기본적으로 수동 슬롯 배치 사용
        base.Awake();
    }

    public override bool AddItem(ARune rune, int quantity = 1)
    {
        // 빈 슬롯 찾기
        for (int i = 0; i < _itemsList.Count; i++)
        {
            if (_itemsList[i] == null)
            {
                _itemsList[i] = new InventoryItem(rune, 1); // 장비 인벤토리는 항상 수량 1
                UpdateSlot(i);
                return true;
            }
        }

        return false; // 빈 슬롯 없음
    }

    public bool AddItemToSlot(ARune rune, int slotIndex, int quantity = 1)
    {
        if (slotIndex < 0 || slotIndex >= _itemsList.Count)
            return false;

        // 슬롯이 비어있는지 확인
        if (_itemsList[slotIndex] == null)
        {
            _itemsList[slotIndex] = new InventoryItem(rune, 1); // 장비 인벤토리는 항상 수량 1
            UpdateSlot(slotIndex);
            return true;
        }

        return false; // 슬롯이 비어있지 않음
    }

    public override bool MoveItem(int fromSlot, int toSlot)
    {
        if (fromSlot < 0 || fromSlot >= _itemsList.Count || toSlot < 0 || toSlot >= _itemsList.Count)
            return false;

        // 일반적인 교환
        InventoryItem temp = _itemsList[toSlot];
        _itemsList[toSlot] = _itemsList[fromSlot];
        _itemsList[fromSlot] = temp;

        UpdateSlot(fromSlot);
        UpdateSlot(toSlot);
        return true;
    }

    public override void OnItemDoubleClick(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _itemsList.Count || _itemsList[slotIndex] == null)
            return;

        // 아이템을 기본 인벤토리로 이동
        ARune rune = _itemsList[slotIndex].Rune;
        if (_basicInventory.AddItem(rune, 1))
        {
            _itemsList[slotIndex] = null;
            UpdateSlot(slotIndex);
        }
    }

    /*
    // 모든 슬롯의 룬 정보를 배열로 반환
    public ARune[] GetEquippedRunes()
    {
        ARune[] equippedRunes = new ARune[_itemsList.Count];
        for (int i = 0; i < _itemsList.Count; i++)
        {
            equippedRunes[i] = _itemsList[i]?.Rune;
        }
        return equippedRunes;
    }*/

    // 특정 슬롯의 룬 정보를 반환
    public ARune GetRuneAtSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _itemsList.Count)
            return null;
            
        return _itemsList[slotIndex]?.Rune;
    }
} 