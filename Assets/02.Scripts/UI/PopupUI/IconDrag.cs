using UnityEngine;
using UnityEngine.EventSystems;

// 드래그 시작, 그래그 중, 드래그가 끝날 때 이벤트 탐지를 위한 인터페이스스
public class IconDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // static으로 선언하는 이유는 Drop이벤트를 가진 슬롯 스크립트에서 접근하기 위해서
    public static GameObject beingDraggedIcon;

    // 백업용 위치
    Vector3 startPosition;

    // Icon 드래그중 변경할 부모 RactTransform
    [SerializeField] Transform onDragParent;

    // 백업용 부모
    public Transform startParent;

    // 인터페이스 IBeginDragHandler를 상속, 드래그 시작했을 때 발생하는 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그가 시작될 때 대상 Icon의 게임오브젝트를 static 변수에 할당
        beingDraggedIcon = gameObject;

        // 백업
        startPosition = transform.position;
        startParent = transform.parent;

        // Drop이벤트를 정상적으로 감지하기 위해 Icon RectTransform을 무시
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        transform.SetParent(onDragParent);
    }

    // 인터페이스 IDragHandler 상속, 드래그 중에 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    // 인터페이스 IEndDragHandler 상속, 드래그가 끝날 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 대상을 지우고 해당 Icon에 이벤트 감지를 허용
        beingDraggedIcon = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // 이동중에 할당 되었던 부모 transform과 같다면 Icon의 부모와 위치를 복구
        if(transform.parent == onDragParent)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
    }
}
