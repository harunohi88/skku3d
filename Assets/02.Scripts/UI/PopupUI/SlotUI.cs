using UnityEngine;
using UnityEngine.EventSystems;

// 드랍이벤트 감지를 위한 인터페이스
public class SlotUI : MonoBehaviour, IDropHandler
{
    private GameObject Icon()
    {
        // 슬롯에 아이템(자식 트랜스폼)이 있으면 아이템의 gameObject를 반환
        // 슬롯에 아이템이 없다면 null을 반환
        if(transform.childCount > 0)
        {
            return transform.GetChild(0).gameObject;
        }
        return null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 슬롯이 비어있다면 Icon을 자식으로 추가, 위치 변경
        if(Icon() == null && IconDrag.beingDraggedIcon != null)
        {
            IconDrag.beingDraggedIcon.transform.SetParent(transform);
            IconDrag.beingDraggedIcon.transform.position = transform.position;
        }
    }
}
