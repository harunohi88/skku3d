using UnityEngine;

public class DifficultySliderUI : MonoBehaviour
{
    [SerializeField] private RectTransform _diffIndexTransform;
    [SerializeField] private RectTransform _diffFrameTransform;
    [SerializeField] private float _playTime = 300f;

    private Vector2 _startPos;
    private Vector2 _endPos;

    private void Start()
    {
        _startPos = _diffIndexTransform.anchoredPosition;
        _endPos = _startPos + new Vector2(-_diffIndexTransform.rect.width, 0f);
    }

    private void Update()
    {
        float t = Mathf.Clamp01(TimeManager.Instance.GetTime() / _playTime);
        _diffIndexTransform.anchoredPosition = Vector2.Lerp(_startPos, _endPos, t);
    }
}
