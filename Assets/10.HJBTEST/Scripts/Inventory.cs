using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    [Item의 상속구조]
    - Item
        - CountableItem
            - PortionItem : IUsableItem.Use() -> 사용 및 수량 1 소모
        - EquipmentItem
            - WeaponItem
            - ArmorItem

    [ItemData의 상속구조]
      (ItemData는 해당 아이템이 공통으로 가질 데이터 필드 모음)
      (개체마다 달라져야 하는 현재 내구도, 강화도 등은 Item 클래스에서 관리)

    - ItemData
        - CountableItemData
            - PortionItemData : 효과량(Value : 회복량, 공격력 등에 사용)
        - EquipmentItemData : 최대 내구도
            - WeaponItemData : 기본 공격력
            - ArmorItemData : 기본 방어력
*/

/*
    [API]
    - bool HasItem(int) : 해당 인덱스의 슬롯에 아이템이 존재하는지 여부
    - bool IsCountableItem(int) : 해당 인덱스의 아이템이 셀 수 있는 아이템인지 여부
    - int GetCurrentAmount(int) : 해당 인덱스의 아이템 수량
        - -1 : 잘못된 인덱스
        -  0 : 빈 슬롯
        -  1 : 셀 수 없는 아이템이거나 수량 1
    - ItemData GetItemData(int) : 해당 인덱스의 아이템 정보
    - string GetItemName(int) : 해당 인덱스의 아이템 이름

    - int Add(ItemData, int) : 해당 타입의 아이템을 지정한 개수만큼 인벤토리에 추가
        - 자리 부족으로 못넣은 개수만큼 리턴(0이면 모두 추가 성공했다는 의미)
    - void Remove(int) : 해당 인덱스의 슬롯에 있는 아이템 제거
    - void Swap(int, int) : 두 인덱스의 아이템 위치 서로 바꾸기
    - void SeparateAmount(int a, int b, int amount)
        - a 인덱스의 아이템이 셀 수 있는 아이템일 경우, amount만큼 분리하여 b 인덱스로 복제
    - void Use(int) : 해당 인덱스의 아이템 사용
    - void UpdateSlot(int) : 해당 인덱스의 슬롯 상태 및 UI 갱신
    - void UpdateAllSlot() : 모든 슬롯 상태 및 UI 갱신
    - void UpdateAccessibleStatesAll() : 모든 슬롯 UI에 접근 가능 여부 갱신
    - void TrimAll() : 앞에서부터 아이템 슬롯 채우기
    - void SortAll() : 앞에서부터 아이템 슬롯 채우면서 정렬
*/

// 날짜 : 2021-03-07 PM 7:33:52
// 작성자 : Rito

namespace Rito.InventorySystem
{
    public enum InventoryType
    {
        Normal,     // 일반 인벤토리 (아이템 합치기 가능)
        Equipment   // 장착 인벤토리 (아이템 합치기 불가)
    }

    public class Inventory : MonoBehaviour
    {
        /***********************************************************************
        *                               Private Fields
        ***********************************************************************/
        #region .

        [SerializeField]
        private InventoryUI _inventoryUI; // 연결된 인벤토리 UI

        [SerializeField]
        private InventoryType _inventoryType = InventoryType.Normal; // 인벤토리 타입

        /// <summary> 아이템 목록 </summary>
        [SerializeField]
        private RuneItem[] _items;

        /// <summary> 업데이트 할 인덱스 목록 </summary>
        private readonly HashSet<int> _indexSetForUpdate = new HashSet<int>();

        private class ItemComparer : IComparer<RuneItem>
        {
            public int Compare(RuneItem a, RuneItem b)
            {
                return a.ID - b.ID;
            }
        }
        private static readonly ItemComparer _itemComparer = new ItemComparer();
        
        

        public Action UpdateSlotEvent;

        #endregion
        /***********************************************************************
        *                               Unity Events
        ***********************************************************************/
        #region .

        private void Awake()
        {
            _inventoryUI.SetInventoryReference(this);
        }

        private void Start()
        {
            if (_inventoryUI == null)
            {
                Debug.LogError("InventoryUI reference is missing!");
                return;
            }

            _items = new RuneItem[_inventoryUI.SlotCount];
            if (_items.Length == 0)
            {
                Debug.LogError("No slots found in InventoryUI!");
                return;
            }

            UpdateAccessibleStatesAll();
        }

        #endregion
        /***********************************************************************
        *                               Private Methods
        ***********************************************************************/
        #region .
        /// <summary> 인덱스가 수용 범위 내에 있는지 검사 </summary>
        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < _items.Length;
        }

        /// <summary> 앞에서부터 비어있는 슬롯 인덱스 탐색 </summary>
        private int FindEmptySlotIndex(int startIndex = 0)
        {
            for (int i = startIndex; i < _items.Length; i++)
                if (_items[i] == null)
                    return i;
            return -1;
        }

        /// <summary> 앞에서부터 개수 여유가 있는 아이템의 슬롯 인덱스 탐색 </summary>
        private int FindCountableItemSlotIndex(RuneData target, int tier, int startIndex = 0)
        {
            for (int i = startIndex; i < _items.Length; i++)
            {
                var current = _items[i];
                if (current == null)
                    continue;

                // 아이템 종류와 티어가 일치하고, 개수 여유 확인
                if (current.RuneData == target && current.Tier == tier)
                {
                    if (!current.IsMax)
                        return i;
                }
            }

            return -1;
        }

        /// <summary> 해당하는 인덱스의 슬롯 상태 및 UI 갱신 </summary>
        private void UpdateSlot(int index)
        {
            if (!IsValidIndex(index)) return;

            RuneItem item = _items[index];

            // 1. 아이템이 슬롯에 존재하는 경우
            if (item != null)
            {
                // 아이콘 등록
                _inventoryUI.SetItemIcon(index, item.IconSprite);

                // 수량이 0인 경우, 아이템 제거
                if (item.IsEmpty)
                {
                    _items[index] = null;
                    RemoveIcon();
                    return;
                }
                // 수량 텍스트 표시
                else
                {
                    _inventoryUI.SetItemAmountText(index, item.Amount);
                }
            }
            // 2. 빈 슬롯인 경우 : 아이콘 제거
            else
            {
                RemoveIcon();
            }

            // 로컬 : 아이콘 제거하기
            void RemoveIcon()
            {
                _inventoryUI.RemoveItem(index);
                _inventoryUI.HideItemAmountText(index); // 수량 텍스트 숨기기
            }
        }

        /// <summary> 해당하는 인덱스의 슬롯들의 상태 및 UI 갱신 </summary>
        private void UpdateSlot(params int[] indices)
        {
            foreach (var i in indices)
            {
                UpdateSlot(i);
            }
            // 모든 슬롯 업데이트가 완료된 후 한 번만 이벤트 발생
            UpdateSlotEvent?.Invoke();
        }

        /// <summary> 모든 슬롯들의 상태를 UI에 갱신 </summary>
        public void UpdateAllSlot()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                UpdateSlot(i);
            }
            // 모든 슬롯 업데이트가 완료된 후 한 번만 이벤트 발생
            UpdateSlotEvent?.Invoke();
        }

        #endregion
        /***********************************************************************
        *                               Check & Getter Methods
        ***********************************************************************/
        #region .

        /// <summary> 해당 슬롯이 아이템을 갖고 있는지 여부 </summary>
        public bool HasItem(int index)
        {
            return IsValidIndex(index) && _items[index] != null;
        }

        /// <summary> 해당 슬롯이 셀 수 있는 아이템인지 여부 </summary>
        public bool IsCountableItem(int index)
        {
            return HasItem(index);
        }

        /// <summary> 
        /// 해당 슬롯의 현재 아이템 개수 리턴
        /// <para/> - 잘못된 인덱스 : -1 리턴
        /// <para/> - 빈 슬롯 : 0 리턴
        /// <para/> - 아이템이 있는 경우 : 현재 수량 리턴
        /// </summary>
        public int GetCurrentAmount(int index)
        {
            if (!IsValidIndex(index)) return -1;
            if (_items[index] == null) return 0;

            return _items[index].Amount;
        }

        /// <summary> 해당 슬롯의 아이템 정보 리턴 </summary>
        public RuneItem GetItemData(int index)
        {
            if (!IsValidIndex(index)) return null;
            if (_items[index] == null) return null;

            return _items[index];
        }

        /// <summary> 해당 슬롯의 아이템 이름 리턴 </summary>
        public string GetItemName(int index)
        {
            if (!IsValidIndex(index)) return "";
            if (_items[index] == null) return "";

            return _items[index].Name;
        }

        #endregion
        /***********************************************************************
        *                               Public Methods
        ***********************************************************************/
        #region .
        /// <summary> 인벤토리 UI 연결 </summary>
        public void ConnectUI(InventoryUI inventoryUI)
        {
            _inventoryUI = inventoryUI;
            _inventoryUI.SetInventoryReference(this);
        }

        /// <summary> 인벤토리에 아이템 추가
        /// <para/> 넣는 데 실패한 잉여 아이템 개수 리턴
        /// <para/> 리턴이 0이면 넣는데 모두 성공했다는 의미
        /// </summary>
        public int Add(RuneData runeData, int tier, int amount = 1)
        {
            int index;
            bool findNextCountable = true;
            index = -1;

            while (amount > 0)
            {
                // 1-1. 이미 해당 아이템이 인벤토리 내에 존재하고, 개수 여유 있는지 검사
                if (findNextCountable)
                {
                    index = FindCountableItemSlotIndex(runeData, tier, index + 1);

                    // 개수 여유있는 기존재 슬롯이 더이상 없다고 판단될 경우, 빈 슬롯부터 탐색 시작
                    if (index == -1)
                    {
                        findNextCountable = false;
                    }
                    // 기존재 슬롯을 찾은 경우, 양 증가시키고 초과량 존재 시 amount에 초기화
                    else
                    {
                        RuneItem item = _items[index];
                        amount = item.AddAmountAndGetExcess(amount);

                        UpdateSlot(index);
                    }
                }
                // 1-2. 빈 슬롯 탐색
                else
                {
                    index = FindEmptySlotIndex(index + 1);

                    // 빈 슬롯조차 없는 경우 종료
                    if (index == -1)
                    {
                        break;
                    }
                    // 빈 슬롯 발견 시, 슬롯에 아이템 추가 및 잉여량 계산
                    else
                    {
                        // 새로운 아이템 생성
                        RuneItem item = RuneItemConverter.ConvertToRuneItem(runeData, tier);
                        item.SetAmount(amount);

                        // 슬롯에 추가
                        _items[index] = item;

                        // 남은 개수 계산
                        amount = (amount > item.MaxAmount) ? (amount - item.MaxAmount) : 0;

                        UpdateSlot(index);
                    }
                }
            }

            return amount;
        }

        /// <summary> 해당 슬롯의 아이템 제거 </summary>
        public void Remove(int index)
        {
            if (!IsValidIndex(index)) return;

            _items[index] = null;
            _inventoryUI.RemoveItem(index);
        }

        /// <summary> 두 인덱스의 아이템 위치를 서로 교체 </summary>
        public void Swap(int indexA, int indexB)
        {
            if (!IsValidIndex(indexA)) return;
            if (!IsValidIndex(indexB)) return;

            RuneItem itemA = _items[indexA];
            RuneItem itemB = _items[indexB];

            // 1. 일반 인벤토리이고 동일한 아이템일 경우 개수 합치기
            if (_inventoryType == InventoryType.Normal && 
                itemA != null && itemB != null &&
                itemA.RuneData == itemB.RuneData &&
                itemA.Tier == itemB.Tier)
            {
                int maxAmount = itemB.MaxAmount;
                int sum = itemA.Amount + itemB.Amount;

                if (sum <= maxAmount)
                {
                    itemA.SetAmount(0);
                    itemB.SetAmount(sum);
                }
                else
                {
                    itemA.SetAmount(sum - maxAmount);
                    itemB.SetAmount(maxAmount);
                }
            }
            // 2. 장착 인벤토리이거나 다른 아이템일 경우 : 슬롯 교체
            else
            {
                _items[indexA] = itemB;
                _items[indexB] = itemA;
            }

            // 두 슬롯 정보 갱신
            UpdateSlot(indexA, indexB);
        }

        /// <summary> 아이템의 수량 나누기(A -> B 슬롯으로) </summary>
        public void SeparateAmount(int indexA, int indexB, int amount)
        {
            if(!IsValidIndex(indexA)) return;
            if(!IsValidIndex(indexB)) return;

            RuneItem itemA = _items[indexA];
            RuneItem itemB = _items[indexB];

            // 조건 : A 슬롯 - 아이템 있음 / B 슬롯 - Null
            // 조건에 맞는 경우, 복제하여 슬롯 B에 추가
            if (itemA != null && itemB == null)
            {
                _items[indexB] = itemA.SeperateAndClone(amount);

                UpdateSlot(indexA, indexB);
            }
        }

        /// <summary> 해당 슬롯의 아이템 사용 </summary>
        public void Use(int index)
        {
            if (!IsValidIndex(index)) return;
            if (_items[index] == null) return;

            // 아이템 사용
            bool succeeded = _items[index].Use();

            if (succeeded)
            {
                UpdateSlot(index);
            }
        }

        /// <summary> 모든 슬롯 UI에 접근 가능 여부 업데이트 </summary>
        public void UpdateAccessibleStatesAll()
        {
            _inventoryUI.SetAccessibleSlotRange(_items.Length);
        }

        /// <summary> 빈 슬롯 없이 앞에서부터 채우기 </summary>
        public void TrimAll()
        {
            _indexSetForUpdate.Clear();

            int i = -1;
            while (_items[++i] != null) ;
            int j = i;

            while (true)
            {
                while (++j < _items.Length && _items[j] == null);

                if (j == _items.Length)
                    break;

                _indexSetForUpdate.Add(i);
                _indexSetForUpdate.Add(j);

                _items[i] = _items[j];
                _items[j] = null;
                i++;
            }

            foreach (var index in _indexSetForUpdate)
            {
                UpdateSlot(index);
            }
        }

        /// <summary> 빈 슬롯 없이 채우면서 아이템 종류별로 정렬하기 </summary>
        public void SortAll()
        {
            // 1. Trim
            int i = -1;
            while (_items[++i] != null) ;
            int j = i;

            while (true)
            {
                while (++j < _items.Length && _items[j] == null) ;

                if (j == _items.Length)
                    break;

                _items[i] = _items[j];
                _items[j] = null;
                i++;
            }

            // 2. Sort
            Array.Sort(_items, 0, i, _itemComparer);

            // 3. Update
            UpdateAllSlot();
        }

        /// <summary> 특정 슬롯에 아이템 추가 (성공 시 0, 실패 시 남은 수량 반환) </summary>
        public int AddToSlot(RuneItem item, int slotIndex)
        {
            if (!IsValidIndex(slotIndex)) return item.Amount;
            if (_items[slotIndex] != null) return item.Amount; // 이미 아이템이 있으면 추가 불가(혹은 교환 등 추가 구현 가능)

            int addAmount = Mathf.Min(item.Amount, item.MaxAmount);
            item.SetAmount(addAmount);
            _items[slotIndex] = item;
            UpdateSlot(slotIndex);
            return item.Amount - addAmount;
        }

        /// <summary> 두 인벤토리의 슬롯끼리 아이템을 교환 </summary>
        public void SwapWithOtherInventory(int myIndex, Inventory other, int otherIndex)
        {
            if (!IsValidIndex(myIndex) || !other.IsValidIndex(otherIndex)) return;
            var temp = _items[myIndex];
            _items[myIndex] = other._items[otherIndex];
            other._items[otherIndex] = temp;
            UpdateSlot(myIndex);
            other.UpdateSlot(otherIndex);
        }

        #endregion
    }
}