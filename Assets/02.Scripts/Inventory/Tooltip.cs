using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _tierText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasScaler _canvasScaler;

    public Vector3 Offset = new Vector3(0, 0, 0);

    public void Show(string name, string tier, string description, RectTransform slotRect)
    {
        _nameText.text = name;
        _tierText.text = $"티어 {tier}";
        _descriptionText.text = description;
        gameObject.SetActive(true);
        SetRectPosition(slotRect);
    }

    /// <summary> 툴팁의 위치 조정 </summary>
    public void SetRectPosition(RectTransform slotRect)
    {
        // 캔버스 스케일러에 따른 해상도 대응
        float wRatio = Screen.width / _canvasScaler.referenceResolution.x;
        float hRatio = Screen.height / _canvasScaler.referenceResolution.y;
        float ratio =
            wRatio * (1f - _canvasScaler.matchWidthOrHeight) +
            hRatio * (_canvasScaler.matchWidthOrHeight);

        float slotWidth = slotRect.rect.width * ratio + Offset.x;
        float slotHeight = slotRect.rect.height * ratio + Offset.y;

        // 툴팁 초기 위치(슬롯 우하단) 설정
        _rectTransform.position = slotRect.position + new Vector3(slotWidth, -slotHeight);
        Vector2 pos = _rectTransform.position;

        // 툴팁의 크기
        float width = _rectTransform.rect.width * ratio;
        float height = _rectTransform.rect.height * ratio;

        // 우측, 하단이 잘렸는지 여부
        bool rightTruncated = pos.x + width > Screen.width;
        bool bottomTruncated = pos.y - height < 0f;

        // 오른쪽만 잘림 => 슬롯의 Left Bottom 방향으로 표시
        if (rightTruncated && !bottomTruncated)
        {
            _rectTransform.position = new Vector2(pos.x - width - slotWidth, pos.y);
        }
        // 아래쪽만 잘림 => 슬롯의 Right Top 방향으로 표시
        else if (!rightTruncated && bottomTruncated)
        {
            _rectTransform.position = new Vector2(pos.x, pos.y + height + slotHeight);
        }
        // 모두 잘림 => 슬롯의 Left Top 방향으로 표시
        else if (rightTruncated && bottomTruncated)
        {
            _rectTransform.position = new Vector2(pos.x - width - slotWidth, pos.y + height + slotHeight);
        }
        // 잘리지 않음 => 슬롯의 Right Bottom 방향으로 표시 (Do Nothing)
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
} 