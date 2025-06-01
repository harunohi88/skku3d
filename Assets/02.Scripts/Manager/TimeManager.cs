using System;
using UnityEngine;

public class TimeManager : BehaviourSingleton<TimeManager>
{
    public int Difficulty;
    public string DifficultyText;
    public EnemyDifficultyMultiplier DifficultyMultiplier;

    [SerializeField] private float _time;
    private float _difficultyChangeTime = 300f;
    private int _currentTID;

    public Action OnDifficultyChanged;

    private void Start()
    {
        _time = 0;
        _currentTID = 10000;

        Global.Instance.OnDataLoaded += LoadTimeData;
        DontDestroyOnLoad(gameObject);
    }

    private void LoadTimeData()
    {
        TimeData data = DataTable.Instance.GetTimeData(_currentTID);
        Difficulty = data.DifficultyNum;
        DifficultyText = data.DifficultyText;
        DifficultyMultiplier = new EnemyDifficultyMultiplier(data.EnemyCountMultiplier, data.EnemyHealthMultiplier, data.EnemyDamageMultiplier);

        if (DataTable.Instance.GetTimeData(_currentTID + 1) != null)
        {
            _difficultyChangeTime = DataTable.Instance.GetTimeData(_currentTID + 1).Time;
        }
    }

    public float GetTime() => _time;

    public void Update()
    {
        _time += Time.deltaTime;

        if (_time > _difficultyChangeTime)
        {
            _currentTID++;
            LoadTimeData();

            OnDifficultyChanged?.Invoke();
        }
    }
}
