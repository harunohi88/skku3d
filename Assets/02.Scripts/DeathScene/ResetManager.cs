using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnClickRestart()
    {
        Debug.Log("OnClickRestart called");
        Time.timeScale = 1f;
        StartCoroutine(ResetRoutine());
    }

    private IEnumerator ResetRoutine()
    {
        // 1. EmptyScene 로드 (기존 씬들은 전부 언로드됨)
        yield return SceneManager.LoadSceneAsync("EmptyScene", LoadSceneMode.Single);

        // 2. 1 프레임 대기 (씬 오브젝트 전부 파괴 완료 보장)
        yield return null;

        // 3. DontDestroyOnLoad 오브젝트 제거
        var targets = FindObjectsByType<AutoDestroyOnSceneReset>(FindObjectsSortMode.None);
        foreach (var t in targets)
        {
            Destroy(t.gameObject);
        }

        // 4. 다시 한 프레임 대기 (파괴 안정성 확보)
        yield return null;

        // 5. 초기 씬 로드
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }
}