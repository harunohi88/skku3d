using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class HUDUI : BehaviourSingleton<HUDUI>
{
    public TextMeshProUGUI stageText;
    public GameObject BossHealthBar;
    public Image HitImage;

    public float HitDuration = 0.4f;

    private void Awake()
    {
        SceneManager.sceneLoaded += RefreshStageText;
        SceneManager.sceneLoaded += CheckBossHPShow;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= RefreshStageText;
        SceneManager.sceneLoaded -= CheckBossHPShow;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void RefreshStageText(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1) stageText.text = "스테이지 1";
        else if (scene.buildIndex == 2) stageText.text = "스테이지 1 \n보스";
        else if (scene.buildIndex == 3) stageText.text = "스테이지 2";
        else if (scene.buildIndex == 4) stageText.text = "스테이지 2 \n보스";
        else if (scene.buildIndex == 5) stageText.text = "스테이지 3 \n보스";
    }

    public void CheckBossHPShow(Scene scene, LoadSceneMode mode)
    {
        if (BossHealthBar == null) return;
        
        if (SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex) ==
            SceneManager.GetSceneByName("Stage1_Boss")
            || SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex) ==
            SceneManager.GetSceneByName("Stage2_Boss")
            || SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex) ==
            SceneManager.GetSceneByName("Stage3_Boss"))
        {
            BossHealthBar.SetActive(true);
        }
        else
        {
            BossHealthBar.SetActive(false);
        }
    }

    public void ShowDamageVignette()
    {
        StopAllCoroutines();
        StartCoroutine(VignetteFadeCoroutine());
    }

    private IEnumerator VignetteFadeCoroutine()
    {
        // Fade In
        float elapsed = 0f;
        Color color = HitImage.color;
        while (elapsed < HitDuration / 2f)
        {
            float alpha = Mathf.Lerp(0, 1f, elapsed / (HitDuration / 2f));
            HitImage.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        HitImage.color = new Color(color.r, color.g, color.b, 1f);

        // Fade Out
        elapsed = 0f;
        while (elapsed < HitDuration)
        {
            float alpha = Mathf.Lerp(1f, 0, elapsed / HitDuration);
            HitImage.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        HitImage.color = new Color(color.r, color.g, color.b, 0);
    }
}
