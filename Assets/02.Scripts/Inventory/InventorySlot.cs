using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _quantityText;
    [SerializeField] private GameObject _highlightObject;
    [SerializeField] private TextMeshProUGUI _tooltipNameText;
    [SerializeField] private TextMeshProUGUI _tooltipDescriptionText;

    private BaseInventory _inventory;
    private int _slotIndex;
    public InventoryItem CurrentItem;
    private bool _isDragging;
    private Vector2 _dragOffset;

    private Tooltip _tooltip;

    public void Initialize(BaseInventory inv, int index, Tooltip tooltip)
    {
        _inventory = inv;
        _slotIndex = index;
        _highlightObject.SetActive(false);
        _tooltip = tooltip;
    }

    public void UpdateSlot(InventoryItem item)
    {
        CurrentItem = item;
        
        if (item != null && !item.IsEmpty())
        {
            _itemImage.sprite = item.Rune.Sprite;
            _itemImage.enabled = true;
            _quantityText.text = item.Quantity > 1 ? item.Quantity.ToString() : "";
            _quantityText.enabled = item.Quantity > 1;
        }
        else
        {
            _itemImage.enabled = false;
            _quantityText.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CurrentItem != null && !CurrentItem.IsEmpty())
        {
            _highlightObject.SetActive(true);
            if (_tooltip != null)
            {
                _tooltip.Show($"Rune T{CurrentItem.Rune.TierValue}", CurrentItem.Rune.RuneDescription, transform as RectTransform);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _highlightObject.SetActive(false);
        if (_tooltip != null)
            _tooltip.Hide();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CurrentItem != null && !CurrentItem.IsEmpty())
        {
            _isDragging = true;
            _dragOffset = eventData.position - (Vector2)transform.position;
            _itemImage.transform.SetParent(transform.root);
            _itemImage.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDragging)
        {
            _itemImage.transform.position = eventData.position - _dragOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isDragging)
        {
            _isDragging = false;
            _itemImage.transform.SetParent(transform);
            _itemImage.transform.localPosition = Vector3.zero;
            _itemImage.raycastTarget = true;

            GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;
            Debug.Log($"OnEndDrag - Hit Object: {(hitObject != null ? hitObject.name : "null")}");
            
            if (hitObject != null)
            {
                InventorySlot targetSlot = hitObject.GetComponentInParent<InventorySlot>();
                Debug.Log($"OnEndDrag - Target Slot: {(targetSlot != null ? "Found" : "Not Found")}");
                
                if (targetSlot != null)
                {
                    // 대상 인벤토리 가져오기
                    BaseInventory targetInventory = targetSlot.GetComponentInParent<BaseInventory>();
                    Debug.Log($"OnEndDrag - Target Inventory: {(targetInventory != null ? targetInventory.GetType().Name : "null")}");
                    Debug.Log($"OnEndDrag - Source Inventory: {_inventory.GetType().Name}");
                    
                    if (targetInventory != null)
                    {
                        // 대상이 다른 인벤토리에 있으면 소스 인벤토리의 MoveItem 사용
                        if (targetInventory != _inventory)
                        {
                            Debug.Log($"OnEndDrag - 서로 다른 인벤토리 간 이동");
                            if (_inventory is BasicInventory basicInv && targetInventory is EquipInventory)
                            {
                                basicInv.MoveItemToEquip(_slotIndex, targetSlot._slotIndex);
                            }
                            else
                            {
                                _inventory.MoveItem(_slotIndex, targetSlot._slotIndex);
                            }
                        }
                        else
                        {
                            Debug.Log($"OnEndDrag - 같은 인벤토리 내 이동");
                            _inventory.MoveItem(_slotIndex, targetSlot._slotIndex);
                        }
                    }
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2 && CurrentItem != null && !CurrentItem.IsEmpty())
        {
            _inventory.OnItemDoubleClick(_slotIndex);
        }
    }
} 