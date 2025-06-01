using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : BehaviourSingleton<GameManager>
{
    private int _currentStage = 1;
    public bool IsStart = false;
    public float MoveDuration = 2f;
    public float FocusDistance = 22f;
    public GameObject startCanvas;
    public CanvasGroup HUDCanvasGroup;

    public Transform PlayerCameraTransform;

    public Volume PPVolume;

    public int GetCurrentStage() => _currentStage;

    public void StartGame()
    {
        // 위치 이동
        Camera.main.transform.DOMove(PlayerCameraTransform.position, MoveDuration)
                 .SetEase(Ease.OutCubic);

        // 회전 이동
        Camera.main.transform.DORotateQuaternion(PlayerCameraTransform.rotation, MoveDuration)
                 .SetEase(Ease.OutCubic).OnComplete(() =>
                 {
                     IsStart = true; 
                     startCanvas.SetActive(false);
                 });
        HUDCanvasGroup.gameObject.SetActive(true);
        DOTween.To(() => HUDCanvasGroup.alpha, x => HUDCanvasGroup.alpha = x, 1, MoveDuration);

        if (PPVolume.profile.TryGet<DepthOfField>(out var dof))
        {
            dof.focusDistance.value = 10f;
            DOTween.To(() => dof.focusDistance.value, x => dof.focusDistance.value = x, 22f, MoveDuration);
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
}
