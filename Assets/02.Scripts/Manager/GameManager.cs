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
    private int _PortalTriggerCount = 0;

    public Volume PPVolume;

    public int GetCurrentStage() => _currentStage;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += PlayBGMWhenStart;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayBGMWhenStart(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex - 1) == SceneManager.GetSceneByName("Stage1_Boss")
            || SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex - 1) == SceneManager.GetSceneByName("Stage2_Boss")
            || SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex - 1) == SceneManager.GetSceneByName("Stage3_Boss"))
        {
            if(AudioManager.Instance.CheckCurrentBGM(0) == false)
            {
                AudioManager.Instance.PlayBGM(0);
            }
        }
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

    public void GoToNextStage()
    {
        _PortalTriggerCount++;
        if (_PortalTriggerCount == 2)
        {
            _currentStage++;
            _PortalTriggerCount = 0;
        }
    }
}
