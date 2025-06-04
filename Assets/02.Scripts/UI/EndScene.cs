using JetBrains.Annotations;
using UnityEngine;
using TMPro;

public class EndScene : MonoBehaviour
{
    public TextMeshProUGUI SurviveText;
    public void Start()
    {
        float currentTime = TimeManager.Instance.GetTime();
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        SurviveText.text = $"생존 시간 : {minutes:00}분 {seconds:00}초";
    }
    public void OnClick()
    {
        GameManager.Instance.GoToMainScene();
    }
}
