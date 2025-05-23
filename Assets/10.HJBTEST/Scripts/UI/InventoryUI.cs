using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryUI : MonoBehaviour
{
    /***********************************************************************
    *                               Option Fields
    ***********************************************************************/
    #region .
    [Header("Options")]
    public bool IsDynamicInitSlot = true;
    [Range(0, 10)]
    [SerializeField] private int _horizontalSlotCount = 8;  // 슬롯 가로 개수
    [Range(0, 10)]
    [SerializeField] private int _verticalSlotCount = 8;      // 슬롯 세로 개수
    [SerializeField] private float _slotMargin = 8f;          // 한 슬롯의 상하좌우 여백
    [SerializeField] private float _contentAreaPadding = 20f; // 인벤토리 영역의 내부 여백
    [Range(32, 128)]
    [SerializeField] private float _slotSize = 64f;      // 각 슬롯의 크기

    [Space]
    [SerializeField] private bool _showTooltip = true;
    [SerializeField] private bool _showHighlight = true;
    [SerializeField] private bool _showRemovingPopup = true;

    [Header("Connected Objects")]
    [SerializeField] private RectTransform _contentAreaRT; // 슬롯들이 위치할 영역
    [SerializeField] private GameObject _slotUiPrefab;     // 슬롯의 원본 프리팹
    [SerializeField] private ItemTooltipUI _itemTooltip;   // 아이템 정보를 보여줄 툴팁 UI
    [SerializeField] private InventoryPopupUI _popup;      // 팝업 UI 관리 객체

    [Header("Buttons")]
    [SerializeField] private Button _trimButton;
    [SerializeField] private Button _sortButton;

    /*[Header("Filter Toggles")]
    [SerializeField] private Toggle _toggleFilterAll;
    [SerializeField] private Toggle _toggleFilterEquipments;
    [SerializeField] private Toggle _toggleFilterPortions;*/

    [Space(16)]
    [SerializeField] private bool _mouseReversed = false; // 마우스 클릭 반전 여부

    [Header("Drag Icon Root (드래그 아이콘을 올릴 부모)")]
    public Transform dragIconRoot;

    #endregion
    /***********************************************************************
    *                               Private Fields
    ***********************************************************************/
    #region .

    /// <summary> 연결된 인벤토리 </summary>
    private Inventory _inventory;

    [SerializeField]
    private List<ItemSlotUI> _slotUIList = new List<ItemSlotUI>();
    public int SlotCount => _slotUIList.Count;
    private GraphicRaycaster _gr;
    private PointerEventData _ped;
    private List<RaycastResult> _rrList;

    private ItemSlotUI _pointerOverSlot; // 현재 포인터가 위치한 곳의 슬롯
    private ItemSlotUI _beginDragSlot; // 현재 드래그를 시작한 슬롯
    private Transform _beginDragIconTransform; // 해당 슬롯의 아이콘 트랜스폼

    private int _leftClick = 0;
    private int _rightClick = 1;

    private Vector3 _beginDragIconPoint;   // 드래그 시작 시 슬롯의 위치
    private Vector3 _beginDragCursorPoint; // 드래그 시작 시 커서의 위치
    private int _beginDragSlotSiblingIndex;
    
    /// <summary> 인벤토리 UI 내 아이템 필터링 옵션 </summary>
    /*private enum FilterOption
    {
        All, Equipment, Portion
    }
    private FilterOption _currentFilterOption = FilterOption.All;*/

    private InventoryUI _otherInventoryUI;
    private Inventory _otherInventory;

    private Transform _beginDragIconOriginalParent;

    // 더블클릭 관련 변수들
    private float _lastClickTime;
    private const float DOUBLE_CLICK_TIME = 0.3f;
    private ItemSlotUI _lastClickedSlot;

    #endregion
    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region .
    private void Awake()
    {
        Init();
        InitSlots();
        //InitButtonEvents();
        //InitToggleEvents();
    }

    private void Update()
    {
        _ped.position = Input.mousePosition;

        OnPointerEnterAndExit();
        if(_showTooltip) ShowOrHideItemTooltip();
        OnPointerDown();
        OnPointerDrag();
        OnPointerUp();
    }

    #endregion
    /***********************************************************************
    *                               Init Methods
    ***********************************************************************/
    #region .
    private void Init()
    {
        TryGetComponent(out _gr);
        if (_gr == null)
            _gr = gameObject.AddComponent<GraphicRaycaster>();

        // Graphic Raycaster
        _ped = new PointerEventData(EventSystem.current);
        _rrList = new List<RaycastResult>(10);

        // Item Tooltip UI
        if (_itemTooltip == null)
        {
            _itemTooltip = GetComponentInChildren<ItemTooltipUI>();
            EditorLog("인스펙터에서 아이템 툴팁 UI를 직접 지정하지 않아 자식에서 발견하여 초기화하였습니다.");
        }
    }

    /// <summary> 지정된 개수만큼 슬롯 영역 내에 슬롯들 동적 생성 </summary>
    private void InitSlots()
    {
        // 수동 배치된 슬롯들을 찾아서 초기화
        if (!IsDynamicInitSlot)
        {
            _slotUIList.Clear();
            var slots = GetComponentsInChildren<ItemSlotUI>();
            foreach (var slot in slots)
            {
                slot.SetSlotIndex(_slotUIList.Count);
                _slotUIList.Add(slot);
            }
            return;
        }

        // 동적 생성 코드는 그대로 유지
        _slotUiPrefab.TryGetComponent(out RectTransform slotRect);
        slotRect.sizeDelta = new Vector2(_slotSize, _slotSize);

        _slotUiPrefab.TryGetComponent(out ItemSlotUI itemSlot);
        if (itemSlot == null)
            _slotUiPrefab.AddComponent<ItemSlotUI>();

        _slotUiPrefab.SetActive(false);

        // --
        Vector2 beginPos = new Vector2(_contentAreaPadding, -_contentAreaPadding);
        Vector2 curPos = beginPos;

        _slotUIList = new List<ItemSlotUI>(_verticalSlotCount * _horizontalSlotCount);

        // 슬롯들 동적 생성
        for (int j = 0; j < _verticalSlotCount; j++)
        {
            for (int i = 0; i < _horizontalSlotCount; i++)
            {
                int slotIndex = (_horizontalSlotCount * j) + i;

                var slotRT = CloneSlot();
                slotRT.pivot = new Vector2(0f, 1f); // Left Top
                slotRT.anchoredPosition = curPos;
                slotRT.gameObject.SetActive(true);
                slotRT.gameObject.name = $"Item Slot [{slotIndex}]";

                var slotUI = slotRT.GetComponent<ItemSlotUI>();
                slotUI.SetSlotIndex(slotIndex);
                _slotUIList.Add(slotUI);

                // Next X
                curPos.x += (_slotMargin + _slotSize);
            }

            // Next Line
            curPos.x = beginPos.x;
            curPos.y -= (_slotMargin + _slotSize);
        }

        // 슬롯 프리팹 - 프리팹이 아닌 경우 파괴
        if(_slotUiPrefab.scene.rootCount != 0)
            Destroy(_slotUiPrefab);

        // -- Local Method --
        RectTransform CloneSlot()
        {
            GameObject slotGo = Instantiate(_slotUiPrefab);
            RectTransform rt = slotGo.GetComponent<RectTransform>();
            rt.SetParent(_contentAreaRT);

            return rt;
        }
    }
    private void InitButtonEvents()
    {
        _trimButton.onClick.AddListener(() => _inventory.TrimAll());
        _sortButton.onClick.AddListener(() => _inventory.SortAll());
    }

    /*
    private void InitToggleEvents()
    {
        _toggleFilterAll.onValueChanged.AddListener(       flag => UpdateFilter(flag, FilterOption.All));
        _toggleFilterEquipments.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.Equipment));
        _toggleFilterPortions.onValueChanged.AddListener(  flag => UpdateFilter(flag, FilterOption.Portion));

        // Local Method
        void UpdateFilter(bool flag, FilterOption option)
        {
            if (flag)
            {
                _currentFilterOption = option;
                UpdateAllSlotFilters();
            }
        }
    }*/

    #endregion
    /***********************************************************************
    *                               Mouse Event Methods
    ***********************************************************************/
    #region .
    private bool IsOverUI()
    {
        if (EventSystem.current == null) return false;
        return EventSystem.current.IsPointerOverGameObject();
    }

    /// <summary> 레이캐스트하여 얻은 첫 번째 UI에서 컴포넌트 찾아 리턴 </summary>
    private T RaycastAndGetFirstComponent<T>() where T : Component
    {
        _rrList.Clear();

        _gr.Raycast(_ped, _rrList);
        
        if(_rrList.Count == 0)
            return null;

        return _rrList[0].gameObject.GetComponent<T>();
    }
    /// <summary> 슬롯에 포인터가 올라가는 경우, 슬롯에서 포인터가 빠져나가는 경우 </summary>
    private void OnPointerEnterAndExit()
    {
        // 이전 프레임의 슬롯
        var prevSlot = _pointerOverSlot;

        // 현재 프레임의 슬롯
        var curSlot = _pointerOverSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

        if (prevSlot == null)
        {
            // Enter
            if (curSlot != null)
            {
                OnCurrentEnter();
            }
        }
        else
        {
            // Exit
            if (curSlot == null)
            {
                OnPrevExit();
            }

            // Change
            else if (prevSlot != curSlot)
            {
                OnPrevExit();
                OnCurrentEnter();
            }
        }

        // ===================== Local Methods ===============================
        void OnCurrentEnter()
        {
            if(_showHighlight)
                curSlot.Highlight(true);
        }
        void OnPrevExit()
        {
            prevSlot.Highlight(false);
        }
    }
    /// <summary> 아이템 정보 툴팁 보여주거나 감추기 </summary>
    private void ShowOrHideItemTooltip()
    {
        // 마우스가 유효한 아이템 아이콘 위에 올라와 있다면 툴팁 보여주기
        bool isValid =
            _pointerOverSlot != null && _pointerOverSlot.HasItem && _pointerOverSlot.IsAccessible
            && (_pointerOverSlot != _beginDragSlot); // 드래그 시작한 슬롯이면 보여주지 않기

        if (isValid)
        {
            UpdateTooltipUI(_pointerOverSlot);
            _itemTooltip.Show();
        }
        else
            _itemTooltip.Hide();
    }
    /// <summary> 슬롯에 클릭하는 경우 </summary>
    private void OnPointerDown()
    {
        // Left Click : Begin Drag
        if (Input.GetMouseButtonDown(_leftClick))
        {
            _beginDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

            // 아이템을 갖고 있는 슬롯만 해당
            if (_beginDragSlot != null && _beginDragSlot.HasItem && _beginDragSlot.IsAccessible)
            {
                // 더블클릭 체크
                float timeSinceLastClick = Time.time - _lastClickTime;
                if (_lastClickedSlot == _beginDragSlot && timeSinceLastClick < DOUBLE_CLICK_TIME)
                {
                    // 더블클릭 처리
                    HandleDoubleClick(_beginDragSlot);
                    _beginDragSlot = null;
                    return;
                }

                _lastClickTime = Time.time;
                _lastClickedSlot = _beginDragSlot;

                EditorLog($"Drag Begin : Slot [{_beginDragSlot.Index}]");

                // 위치 기억, 참조 등록
                _beginDragIconTransform = _beginDragSlot.IconRect.transform;
                _beginDragIconPoint = _beginDragIconTransform.position;
                _beginDragCursorPoint = Input.mousePosition;

                // 드래그 아이콘 부모 기억 및 Canvas 등으로 이동
                _beginDragIconOriginalParent = _beginDragIconTransform.parent;
                if (dragIconRoot != null)
                {
                    _beginDragIconTransform.SetParent(dragIconRoot, true);
                    _beginDragIconTransform.SetAsLastSibling();
                }

                // 맨 위에 보이기 (기존)
                _beginDragSlotSiblingIndex = _beginDragSlot.transform.GetSiblingIndex();
                _beginDragSlot.transform.SetAsLastSibling();

                // 해당 슬롯의 하이라이트 이미지를 아이콘보다 뒤에 위치시키기
                _beginDragSlot.SetHighlightOnTop(false);
            }
            else
            {
                _beginDragSlot = null;
            }
        }

        // 엑티브 아이템이 빠로 없기 때문에 사용하지 않는다.
        /*
        // Right Click : Use Item
        else if (Input.GetMouseButtonDown(_rightClick))
        {
            ItemSlotUI slot = RaycastAndGetFirstComponent<ItemSlotUI>();

            if (slot != null && slot.HasItem && slot.IsAccessible)
            {
                TryUseItem(slot.Index);
            }
        }*/
    }
    /// <summary> 드래그하는 도중 </summary>
    private void OnPointerDrag()
    {
        if(_beginDragSlot == null) return;

        if (Input.GetMouseButton(_leftClick))
        {
            // 위치 이동
            _beginDragIconTransform.position =
                _beginDragIconPoint + (Input.mousePosition - _beginDragCursorPoint);
        }
    }
    /// <summary> 클릭을 뗄 경우 </summary>
    private void OnPointerUp()
    {
        if (Input.GetMouseButtonUp(_leftClick))
        {
            // End Drag
            if (_beginDragSlot != null && _beginDragIconTransform != null)
            {
                // 위치 복원
                _beginDragIconTransform.position = _beginDragIconPoint;

                // 드래그 아이콘 부모 복원
                if (_beginDragIconOriginalParent != null)
                {
                    _beginDragIconTransform.SetParent(_beginDragIconOriginalParent, true);
                    _beginDragIconOriginalParent = null;
                }

                // UI 순서 복원
                _beginDragSlot.transform.SetSiblingIndex(_beginDragSlotSiblingIndex);

                // 드래그 완료 처리
                EndDrag();

                // 해당 슬롯의 하이라이트 이미지를 아이콘보다 앞에 위치시키기
                _beginDragSlot.SetHighlightOnTop(true);

                // 참조 제거
                _beginDragSlot = null;
                _beginDragIconTransform = null;
            }
        }
    }

    private void EndDrag()
    {
        if (_beginDragSlot == null || _inventory == null) return;

        ItemSlotUI endDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

        // Equip 인벤토리 내부 드래그
        if (_inventory._inventoryType == InventoryType.Equipment)
        {
            if (endDragSlot != null && endDragSlot.IsAccessible)
            {
                // Equip 내부에서는 자유롭게 이동/스왑 가능
                TrySwapItems(_beginDragSlot, endDragSlot);
                UpdateTooltipUI(endDragSlot);
            }
            else
            {
                // Equip에서 다른 인벤토리로 드래그 시도 시 원위치
                ResetDragIcon();
            }
            return;
        }

        // Normal 인벤토리에서 Equip으로 드래그
        if (_inventory._inventoryType == InventoryType.Normal && 
            _otherInventory != null && 
            _otherInventory._inventoryType == InventoryType.Equipment)
        {
            var otherSlot = _otherInventoryUI.RaycastAndGetFirstComponent<ItemSlotUI>();
            if (otherSlot != null && otherSlot.IsAccessible)
            {
                // 대상 슬롯이 비어있는 경우에만 한 개씩 이동
                if (!_otherInventory.HasItem(otherSlot.Index))
                {
                    var item = _inventory.GetItemData(_beginDragSlot.Index);
                    if (item != null)
                    {
                        // 한 개만 Equip으로 이동
                        var singleItem = item.SeperateAndClone(1);
                        _otherInventory.AddToSlot(singleItem, otherSlot.Index);
                        
                        // Normal 인벤토리 업데이트 및 정렬
                        _inventory.UpdateSlot(_beginDragSlot.Index);
                        _inventory.SortAll();
                    }
                }
                else
                {
                    // 이미 아이템이 있는 경우 원위치
                    ResetDragIcon();
                }
            }
            return;
        }

        // Normal 인벤토리 내부 드래그
        if (_inventory._inventoryType == InventoryType.Normal)
        {
            // 드래그한 아이템 정보 저장
            RuneItem draggedItem = _inventory.GetItemData(_beginDragSlot.Index);
            if (draggedItem != null)
            {
                // 1. 드래그한 아이템 제거
                _inventory.Remove(_beginDragSlot.Index);

                // 2. 같은 종류의 아이템이 있는지 찾아서 합치기
                int remainingAmount = draggedItem.Amount;
                while (remainingAmount > 0)
                {
                    int index = _inventory.FindCountableItemSlotIndex(draggedItem.RuneData, draggedItem.Tier);
                    if (index == -1) break; // 더 이상 합칠 수 있는 슬롯이 없음

                    RuneItem existingItem = _inventory.GetItemData(index);
                    remainingAmount = existingItem.AddAmountAndGetExcess(remainingAmount);
                    _inventory.UpdateSlot(index);
                }

                // 3. 남은 수량이 있다면 빈 슬롯에 추가
                if (remainingAmount > 0)
                {
                    int emptyIndex = _inventory.FindEmptySlotIndex();
                    if (emptyIndex != -1)
                    {
                        RuneItem newItem = RuneItemConverter.ConvertToRuneItem(draggedItem.RuneData, draggedItem.Tier);
                        newItem.SetAmount(remainingAmount);
                        _inventory.AddToSlot(newItem, emptyIndex);
                    }
                }

                // 4. 정렬
                _inventory.SortAll();
            }
        }

        // 로컬 메서드: 드래그 아이콘 원위치
        void ResetDragIcon()
        {
            if (_beginDragIconTransform != null)
            {
                _beginDragIconTransform.position = _beginDragIconPoint;
                if (_beginDragIconOriginalParent != null)
                {
                    _beginDragIconTransform.SetParent(_beginDragIconOriginalParent, true);
                }
            }
            if (_beginDragSlot != null)
            {
                _beginDragSlot.transform.SetSiblingIndex(_beginDragSlotSiblingIndex);
                _beginDragSlot.SetHighlightOnTop(true);
            }
        }
    }

    private void HandleDoubleClick(ItemSlotUI slot)
    {
        if (_inventory._inventoryType == InventoryType.Equipment)
        {
            // 장착 인벤토리에서 더블클릭 시 normal 인벤토리로 이동
            if (_otherInventory != null && _otherInventory._inventoryType == InventoryType.Normal)
            {
                // 아이템 정보 가져오기
                var item = _inventory.GetItemData(slot.Index);
                if (item != null)
                {
                    // Normal 인벤토리에 Add 메서드로 추가 (자동 합치기)
                    _otherInventory.Add(item.RuneData, item.Tier, item.Amount);
                    
                    // Equipment 인벤토리에서 제거
                    _inventory.Remove(slot.Index);
                }
            }
        }
    }

    #endregion
    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region .

    /// <summary> UI 및 인벤토리에서 아이템 제거 </summary>
    private void TryRemoveItem(int index)
    {
        EditorLog($"UI - Try Remove Item : Slot [{index}]");

        _inventory.Remove(index);
    }

    /// <summary> 아이템 사용 </summary>
    private void TryUseItem(int index)
    {
        EditorLog($"UI - Try Use Item : Slot [{index}]");

        _inventory.Use(index);
    }

    /// <summary> 두 슬롯의 아이템 교환 </summary>
    private void TrySwapItems(ItemSlotUI from, ItemSlotUI to)
    {
        if (from == to)
        {
            EditorLog($"UI - Try Swap Items: Same Slot [{from.Index}]");
            return;
        }

        EditorLog($"UI - Try Swap Items: Slot [{from.Index} -> {to.Index}]");

        // 같은 종류의 아이템이고 같은 티어인 경우 합치기
        var fromItem = _inventory.GetItemData(from.Index);
        var toItem = _inventory.GetItemData(to.Index);
        
        if (fromItem != null && toItem != null && 
            fromItem.RuneData == toItem.RuneData && 
            fromItem.Tier == toItem.Tier)
        {
            // 두 아이템의 수량 합치기
            int totalAmount = fromItem.Amount + toItem.Amount;
            int maxAmount = fromItem.MaxAmount;
            
            if (totalAmount <= maxAmount)
            {
                // 모든 수량을 to 슬롯으로 이동
                _inventory.SeparateAmount(from.Index, to.Index, fromItem.Amount);
            }
            else
            {
                // 최대치까지 채우고 나머지는 from 슬롯에 남김
                int amountToMove = maxAmount - toItem.Amount;
                _inventory.SeparateAmount(from.Index, to.Index, amountToMove);
            }
        }
        else
        {
            // 다른 종류의 아이템이면 교환
            from.SwapOrMoveIcon(to);
            _inventory.Swap(from.Index, to.Index);
        }
    }

    /// <summary> 툴팁 UI의 슬롯 데이터 갱신 </summary>
    private void UpdateTooltipUI(ItemSlotUI slot)
    {
        if(!slot.IsAccessible || !slot.HasItem)
            return;

        // 툴팁 정보 갱신
        var runeItem = _inventory.GetItemData(slot.Index);
        _itemTooltip.SetItemInfo(runeItem);

        // 툴팁 위치 조정
        _itemTooltip.SetRectPosition(slot.SlotRect);
    }

    #endregion
    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region .

    /// <summary> 인벤토리 참조 등록 (인벤토리에서 직접 호출) </summary>
    public void SetInventoryReference(Inventory inventory)
    {
        _inventory = inventory;
    }

    /// <summary> 마우스 클릭 좌우 반전시키기 (true : 반전) </summary>
    public void InvertMouse(bool value)
    {
        _leftClick = value ? 1 : 0;
        _rightClick = value ? 0 : 1;

        _mouseReversed = value;
    }

    /// <summary> 슬롯에 아이템 아이콘 등록 </summary>
    public void SetItemIcon(int index, Sprite icon)
    {
        EditorLog($"Set Item Icon : Slot [{index}]");

        _slotUIList[index].SetItem(icon);
    }

    /// <summary> 해당 슬롯의 아이템 개수 텍스트 지정 </summary>
    public void SetItemAmountText(int index, int amount)
    {
        EditorLog($"Set Item Amount Text : Slot [{index}], Amount [{amount}]");

        // NOTE : amount가 1 이하일 경우 텍스트 미표시
        _slotUIList[index].SetItemAmount(amount);
    }

    /// <summary> 해당 슬롯의 아이템 개수 텍스트 지정 </summary>
    public void HideItemAmountText(int index)
    {
        EditorLog($"Hide Item Amount Text : Slot [{index}]");

        _slotUIList[index].SetItemAmount(1);
    }

    /// <summary> 슬롯에서 아이템 아이콘 제거, 개수 텍스트 숨기기 </summary>
    public void RemoveItem(int index)
    {
        EditorLog($"Remove Item : Slot [{index}]");

        _slotUIList[index].RemoveItem();
    }

    /// <summary> 접근 가능한 슬롯 범위 설정 </summary>
    public void SetAccessibleSlotRange(int accessibleSlotCount)
    {
        // 모든 슬롯이 항상 접근 가능하므로 아무것도 하지 않음
    }

    public void SetOtherInventoryReference(Inventory otherInventory, InventoryUI otherInventoryUI)
    {
        _otherInventory = otherInventory;
        _otherInventoryUI = otherInventoryUI;
    }

    #endregion
    /***********************************************************************
    *                               Editor Only Debug
    ***********************************************************************/
    #region .

    [Header("Editor Options")]
    [SerializeField] private bool _showDebug = true;
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void EditorLog(object message)
    {
        if (!_showDebug) return;
        //UnityEngine.Debug.Log($"[InventoryUI] {message}");
    }

    #endregion
}