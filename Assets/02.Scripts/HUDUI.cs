using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDUI : MonoBehaviour
{
    public TextMeshProUGUI stageText;
    public GameObject BossHealthBar;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += RefreshStageText;
        SceneManager.sceneLoaded += CheckBossHPShow;
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
}
