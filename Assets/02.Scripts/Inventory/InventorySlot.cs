using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private GameObject highlightObject;
    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private TextMeshProUGUI tooltipNameText;
    [SerializeField] private TextMeshProUGUI tooltipDescriptionText;

    private BaseInventory inventory;
    private int slotIndex;
    public InventoryItem currentItem;
    private bool isDragging;
    private Vector2 dragOffset;

    public void Initialize(BaseInventory inv, int index)
    {
        inventory = inv;
        slotIndex = index;
        highlightObject.SetActive(false);
        tooltipObject.SetActive(false);
    }

    public void UpdateSlot(InventoryItem item)
    {
        currentItem = item;
        
        if (item != null && !item.IsEmpty())
        {
            itemImage.sprite = item.Rune.Sprite;
            itemImage.enabled = true;
            quantityText.text = item.Quantity > 1 ? item.Quantity.ToString() : "";
            quantityText.enabled = item.Quantity > 1;
        }
        else
        {
            itemImage.enabled = false;
            quantityText.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null && !currentItem.IsEmpty())
        {
            highlightObject.SetActive(true);
            tooltipObject.SetActive(true);
            tooltipNameText.text = $"Rune T{currentItem.Rune.TierValue}";
            tooltipDescriptionText.text = currentItem.Rune.RuneDescription;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightObject.SetActive(false);
        tooltipObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem != null && !currentItem.IsEmpty())
        {
            isDragging = true;
            dragOffset = eventData.position - (Vector2)transform.position;
            itemImage.transform.SetParent(transform.root);
            itemImage.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            itemImage.transform.position = eventData.position - dragOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;
            itemImage.transform.SetParent(transform);
            itemImage.transform.localPosition = Vector3.zero;
            itemImage.raycastTarget = true;

            GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;
            Debug.Log($"OnEndDrag - Hit Object: {(hitObject != null ? hitObject.name : "null")}");
            
            if (hitObject != null)
            {
                InventorySlot targetSlot = hitObject.GetComponent<InventorySlot>();
                Debug.Log($"OnEndDrag - Target Slot: {(targetSlot != null ? "Found" : "Not Found")}");
                
                if (targetSlot != null)
                {
                    // Get the target inventory
                    BaseInventory targetInventory = targetSlot.GetComponentInParent<BaseInventory>();
                    Debug.Log($"OnEndDrag - Target Inventory: {(targetInventory != null ? targetInventory.GetType().Name : "null")}");
                    Debug.Log($"OnEndDrag - Source Inventory: {inventory.GetType().Name}");
                    
                    if (targetInventory != null)
                    {
                        // If target is in a different inventory, use the source inventory's MoveItem
                        if (targetInventory != inventory)
                        {
                            Debug.Log($"OnEndDrag - Moving between different inventories");
                            if (inventory is BasicInventory basicInv && targetInventory is EquipInventory)
                            {
                                basicInv.MoveItemToEquip(slotIndex, targetSlot.slotIndex);
                            }
                            else
                            {
                                inventory.MoveItem(slotIndex, targetSlot.slotIndex);
                            }
                        }
                        else
                        {
                            Debug.Log($"OnEndDrag - Moving within same inventory");
                            inventory.MoveItem(slotIndex, targetSlot.slotIndex);
                        }
                    }
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2 && currentItem != null && !currentItem.IsEmpty())
        {
            inventory.OnItemDoubleClick(slotIndex);
        }
    }
} 