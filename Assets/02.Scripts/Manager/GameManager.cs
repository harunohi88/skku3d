using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : BehaviourSingleton<GameManager>
{
    private int _currentStage = 1;
    public bool IsStart = false;
    public float MoveDuration = 2f;
    public float FocusDistance = 22f;
    public GameObject startCanvas;
    public CanvasGroup HUDCanvasGroup;

    public Volume PPVolume;

    public int GetCurrentStage() => _currentStage;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public float GetEnemyBaseDamage(EnemyType type)
    {
        StageData data = DataTable.Instance.GetStageData(10000 + _currentStage - 1);

        switch (type)
        {
            case EnemyType.Basic:
                return data.BasicBaseDamage;
            case EnemyType.Elite:
                return data.EliteBaseDamage;
        }

        return -1;
    }

    public float  GetEnemyBaseHealth(EnemyType type)
    {
        StageData data = DataTable.Instance.GetStageData(10000 + _currentStage - 1);

        switch (type)
        {
            case EnemyType.Basic:
                return data.BasicBaseHealth;
            case EnemyType.Elite:
                return data.EliteBaseHealth;
        }

        return -1;
    }
}
