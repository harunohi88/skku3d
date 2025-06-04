using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameStartUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RectTransform thisTransform;
    public Image Image;
    public float Duration = 0.2f;

    private void Awake()
    {
        //thisTransform = GetComponent<RectTransform>();
        //Image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        //Color color = new Color(Image.color.r, Image.color.g, Image.color.b, 0.2f);
        //Image.DOColor(color, Duration);
        //thisTransform.DOScale(1.1f, Duration);
    }

    public void OnPointerExit(PointerEventData data)
    {
        //Color color = new Color(Image.color.r, Image.color.g, Image.color.b, 0f);
        //Image.DOColor(color, Duration);
        //thisTransform.DOScale(1.0f, Duration).OnComplete(() => thisTransform.localScale = Vector3.one);
    }

    public void OnPointerClick(PointerEventData data)
    {
        GameManager.Instance.StartGame();
    }
}
