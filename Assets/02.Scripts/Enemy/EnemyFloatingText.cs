using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class EnemyFloatingTextManager : BehaviourSingleton<EnemyFloatingTextManager>
{
    [MMFInspectorButton("TriggerFeedback")]
    public bool TriggerFeedbackButton;
    public int FontSize = 10;

    private TextMeshPro tmp;
    private MMF_Player _myPlayer;
    public MMFloatingTextSpawner TextSpawner;
    private Gradient gradient;
    private GradientColorKey[] colorKey;
    private GradientAlphaKey[] alphaKey;

    private void Start()
    {
        // 시작 시 MMF Player 컴포넌트를 가져옵니다.
        _myPlayer = this.gameObject.GetComponent<MMF_Player>();
    }

    public void TriggerFeedback(float damage, Vector3 position, bool isCritical)
    {
        MMF_FloatingText floatingText = _myPlayer.GetFeedbackOfType<MMF_FloatingText>(); // 플레이어에 하나의 플로팅 텍스트 피드백만 있는 경우 가정, 더 복잡한 방법은 문서 참조


        floatingText.Value = damage.ToString("0");
        floatingText.Intensity = 1f;
        floatingText.ForceColor = false;
        if (isCritical)
        {
            gradient = new Gradient();
            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[2];
            colorKey[0].color = Color.yellow;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.yellow;
            colorKey[1].time = 1.0f;
            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1.0f;
            alphaKey[1].time = 1.0f;
            gradient.SetKeys(colorKey, alphaKey);

            floatingText.ForceColor = true;
            floatingText.AnimateColorGradient = gradient;
            floatingText.Intensity = 1.5f;
        }

        _myPlayer.PlayFeedbacks(position);
    }
}
