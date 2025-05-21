using UnityEngine;
using TMPro;

public class BossEntry : MonoBehaviour
{
    // 보스 입장 조건 UI 텍스트
    public TextMeshProUGUI BossEntryConditionText;
    // TODO : 보스 임장 조건 이미지가 있다면 이미지 필요
    // 스테이지마다 이미지와 필요한 수량이 바뀐다면?


    private void Start() 
    {
        StageManager.Instance.OnBossEntryConditionEvent += UpdateBossEntryConditionUI;
    }

    private void UpdateBossEntryConditionUI(int currentCount, int totalCount)
    {
        // 보스 입장 조건 UI 업데이트
        BossEntryConditionText.text = $"{currentCount} / {totalCount}";
    }
}
