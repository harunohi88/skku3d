using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class EnemyFloatingText : MonoBehaviour
{
    [MMFInspectorButton("TriggerFeedback")]
    public bool TriggerFeedbackButton;
    public int FontSize = 10;

    private TextMeshPro tmp;
    private MMF_Player _myPlayer;
    private Gradient gradient;
    private GradientColorKey[] colorKey;
    private GradientAlphaKey[] alphaKey;

    private void Start()
    {
        // 시작 시 MMF Player 컴포넌트를 가져옵니다.
        _myPlayer = this.gameObject.GetComponent<MMF_Player>();
    }

    public void TriggerFeedback(float damage, bool isCritical)
    {
        MMF_FloatingText floatingText = _myPlayer.GetFeedbackOfType<MMF_FloatingText>(); // 플레이어에 하나의 플로팅 텍스트 피드백만 있는 경우 가정, 더 복잡한 방법은 문서 참조

        if (isCritical) floatingText.Value = $"{damage.ToString("0")}!";
        else floatingText.Value = damage.ToString("0");

        _myPlayer.PlayFeedbacks(this.transform.position);
    }
}
