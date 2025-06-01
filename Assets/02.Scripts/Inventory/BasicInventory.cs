using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicInventory : BaseInventory
{
    [SerializeField] private EquipInventory _equipInventory;
    [SerializeField] private BasicAllInventory _basicAllInventory;

    public override bool AddItem(Rune rune, int quantity = 1)
    {
        // 동일한 룬의 기존 스택 찾기
        for (int i = 0; i < _itemsList.Count; i++)
        {
            if (_itemsList[i] != null && _itemsList[i].Rune.TID == rune.TID)
            {
                _itemsList[i].AddQuantity(quantity);
                UpdateSlot(i);
                return true;
            }
        }

        // 빈 슬롯 찾기
        for (int i = 0; i < _itemsList.Count; i++)
        {
            if (_itemsList[i] == null)
            {
                _itemsList[i] = new InventoryItem(rune, quantity);
                // 룬 스프라이트 적용
                _itemsList[i].Rune.Sprite = InventoryManager.Instance.GetSprite(rune.TID);

                UpdateSlot(i);
                SortInventory();
                return true;
            }
        }

        return false; // 인벤토리가 가득 참
    }

    public bool ReduceItemQuantity(int tid, int quantity)
    {
        for (int i = 0; i < _itemsList.Count; i++)
        {
            if (_itemsList[i] != null && _itemsList[i].Rune.TID == tid)
            {
                _itemsList[i].RemoveQuantity(quantity);
                if (_itemsList[i].Quantity <= 0)
                {
                    _itemsList[i] = null;
                    SortInventory();
                }
                UpdateSlot(i);

                // BasicAllInventory에도 수량 감소 반영
                if (_basicAllInventory != null)
                {
                    _basicAllInventory.ReduceItemQuantity(tid, _itemsList[i]?.Rune.CurrentTier ?? 0, quantity);
                }
                return true;
            }
        }
        return false;
    }

    private void SortInventory()
    {
        // null 아이템 제거
        _itemsList.RemoveAll(item => item == null);
        
        // TID로 정렬
        _itemsList = _itemsList.OrderBy(item => item.Rune.TID).ToList();
        
        // 남은 슬롯을 null로 채우기
        while (_itemsList.Count < _slotCount)
        {
            _itemsList.Add(null);
        }

        // 모든 슬롯 업데이트
        for (int i = 0; i < _slotsList.Count; i++)
        {
            UpdateSlot(i);
        }
    }

    public bool MoveItemToEquip(int fromSlot, int toSlot)
    {
        Debug.Log($"BasicInventory.MoveItemToEquip - From Slot: {fromSlot}, To Slot: {toSlot}");
        
        if (fromSlot < 0 || fromSlot >= _itemsList.Count || _equipInventory == null)
        {
            Debug.Log($"BasicInventory.MoveItemToEquip - 잘못된 슬롯이거나 장비 인벤토리가 설정되지 않음");
            return false;
        }

        // 장비 인벤토리로 아이템 하나 이동
        if (_itemsList[fromSlot] != null && _itemsList[fromSlot].Quantity > 0)
        {
            Debug.Log($"BasicInventory.MoveItemToEquip - 장비 인벤토리로 아이템 이동 시도");
            if (_equipInventory.AddItemToSlot(_itemsList[fromSlot].Rune, toSlot, 1))
            {
                Debug.Log($"BasicInventory.MoveItemToEquip - 장비 인벤토리에 성공적으로 추가됨");
                _itemsList[fromSlot].RemoveQuantity(1);
                if (_itemsList[fromSlot].Quantity <= 0)
                {
                    _itemsList[fromSlot] = null;
                    SortInventory();
                }
                UpdateSlot(fromSlot);
                return true;
            }
            Debug.Log($"BasicInventory.MoveItemToEquip - 장비 인벤토리에 추가 실패");
        }
        return false;
    }

    public override bool MoveItem(int fromSlot, int toSlot)
    {
        Debug.Log($"BasicInventory.MoveItem - From Slot: {fromSlot}, To Slot: {toSlot}");
        
        if (fromSlot < 0 || fromSlot >= _itemsList.Count || toSlot < 0 || toSlot >= _itemsList.Count)
        {
            Debug.Log($"BasicInventory.MoveItem - 잘못된 슬롯 인덱스");
            return false;
        }

        // 두 슬롯에 아이템이 있고 같은 타입이면 스택
        if (_itemsList[fromSlot] != null && _itemsList[toSlot] != null && 
            _itemsList[fromSlot].Rune.TID == _itemsList[toSlot].Rune.TID)
        {
            Debug.Log($"BasicInventory.MoveItem - 아이템 스택");
            _itemsList[toSlot].AddQuantity(_itemsList[fromSlot].Quantity);
            _itemsList[fromSlot] = null;
        }
        else
        {
            Debug.Log($"BasicInventory.MoveItem - 아이템 교환");
            // 일반적인 교환
            InventoryItem temp = _itemsList[toSlot];
            _itemsList[toSlot] = _itemsList[fromSlot];
            _itemsList[fromSlot] = temp;
        }

        UpdateSlot(fromSlot);
        UpdateSlot(toSlot);
        SortInventory();
        return true;
    }
} 