using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private RectTransform _rectTransform;

    public Vector3 Offset = new Vector3(50, 0, 0);

    public void Show(string name, string description, RectTransform slotRect)
    {
        _nameText.text = name;
        _descriptionText.text = description;
        gameObject.SetActive(true);
        // 슬롯 오른쪽 위에 위치하도록 조정 (원하는 위치로 수정 가능)
        Vector3[] corners = new Vector3[4];
        slotRect.GetWorldCorners(corners);
        Vector3 slotPos = corners[2]; // 우상단
        _rectTransform.position = slotPos + Offset;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
} 