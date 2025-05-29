using System.Collections.Generic;
using UnityEngine;

public class EquipInventory : BaseInventory
{
    [SerializeField] private BasicAllInventory _basicInventory;
    public List<Sprite> RuneSpriteList;
    private const int RUNE_SPRITE_START_INDEX = 10000;
    protected override void Awake()
    {
        _autoCreateSlots = false; // 기본적으로 수동 슬롯 배치 사용
        base.Awake();
    }

    public override bool AddItem(Rune rune, int quantity = 1)
    {
        // 빈 슬롯 찾기
        for (int i = 0; i < _itemsList.Count; i++)
        {
            if (_itemsList[i] == null)
            {
                _itemsList[i] = new InventoryItem(rune, 1); // 장비 인벤토리는 항상 수량 1
                _itemsList[i].Rune.Sprite = RuneSpriteList[rune.TID - RUNE_SPRITE_START_INDEX];
                UpdateSlot(i);
                return true;
            }
        }

        return false; // 빈 슬롯 없음
    }

    public bool AddItemToSlot(Rune rune, int slotIndex, int quantity = 1)
    {
        if (slotIndex < 0 || slotIndex >= _itemsList.Count)
            return false;

        // 슬롯이 비어있는지 확인
        if (_itemsList[slotIndex] == null)
        {
            _itemsList[slotIndex] = new InventoryItem(rune, 1); // 장비 인벤토리는 항상 수량 1
            _itemsList[slotIndex].Rune.Sprite = RuneSpriteList[rune.TID - RUNE_SPRITE_START_INDEX];
            UpdateSlot(slotIndex);
            return true;
        }

        return false; // 슬롯이 비어있지 않음
    }

    public override void UpdateSlot(int index)
    {
        base.UpdateSlot(index);
        // TODO: 룬이 장착되고 해제 될 때 마다 함수 호출예정----------------------------------------------------------
        if(_itemsList[index] != null)
        {
            Debug.Log($"Equip Rune : {index} {_itemsList[index].Rune.TID}");
        }
        else
        {
            Debug.Log($"Unequip Rune : {index}");
        }
    }

    public override bool MoveItem(int fromSlot, int toSlot)
    {
        // 같은 슬롯으로 이동 시도 시 실패 처리
        if (fromSlot == toSlot)
            return false;
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
        Rune rune = _itemsList[slotIndex].Rune;
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
    public Rune GetRuneAtSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _itemsList.Count)
            return null;
            
        return _itemsList[slotIndex]?.Rune;
    }
} 