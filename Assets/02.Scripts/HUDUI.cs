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

    public Button InventoryButton;
    public Button MapButton;
    public Button OptionButton;

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

        InventoryButton.onClick.AddListener(OnInventoryButton);
        MapButton.onClick.AddListener(OnMapButton);
        OptionButton.onClick.AddListener(OnOptionButton);
    }

    public void OnInventoryButton()
    {
        InputManager.Instance.ToggleInventory();
    }
    public void OnOptionButton()
    {
        InputManager.Instance.ToggleOption();
    }
    public void OnMapButton()
    {
        InputManager.Instance.ToggleMap();
    }

    public void RefreshStageText(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1) stageText.text = "스테이지 1";
        else if (SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex) == SceneManager.GetSceneByName("Stage1_Boss"))
        { 
            stageText.text = "스테이지 1\n보스"; 
        }
        else if (SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex) == SceneManager.GetSceneByName("Stage2"))
        {
            stageText.text = "스테이지 2";
        }
        else if (SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex) == SceneManager.GetSceneByName("Stage2_Boss"))
        {
            stageText.text = "스테이지 2\n보스";
        }
        else if (SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex) == SceneManager.GetSceneByName("Stage3_Boss"))
        {
            stageText.text = "스테이지 3\n보스";
        }
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
