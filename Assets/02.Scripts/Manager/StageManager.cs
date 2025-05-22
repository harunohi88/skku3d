using System;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    // TODO : 스테이지마다 매니저를 따로 만들것인가. 스테이지 마다 필요한 개수나 그런것들이
    // 달라질 수 있다. 
    // 일단은 스테이지 1 기준으로 만들어둠
    [Header("보스 입장 조건")]
    // 보스 입장 조건 
    [SerializeField]
    public int BossEntryCondition = 10;
    private int _currentBossEntryConditionCount = 0;

    public GameObject BossPrefab;

    // 보스 입장 조건 증가할때마다 호출되는 이벤트
    // UI 업데이트가 구독
    public Action<int, int> OnBossEntryConditionEvent;


    /// <summary> 보스 입장 조건 카운트 증가 </summary>
    public void AddBossEntryConditionCount()
    {
        _currentBossEntryConditionCount++;

        // 보스 입장 조건 증가 이벤트
        OnBossEntryConditionEvent?.Invoke(_currentBossEntryConditionCount, BossEntryCondition);

        // 보스 입장 조건 충족
        if(_currentBossEntryConditionCount >= BossEntryCondition)
        {
            // TODO : 보스 입장 조건 만족시
            // 입장 조건 초기화?
            // 보스 소환
            SpawnBoss();
            // 잡몹 소환은 막는게 좋으려나??
            // 
        }
    }

    /// <summary> 보스 소환 or 보스 장소 열림 </summary>
    private void SpawnBoss()
    {
        // 보스 소환
    }
}
