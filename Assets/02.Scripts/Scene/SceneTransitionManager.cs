using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadScene("InventoryDestoryTest2");
        }
    }
    public void LoadScene(string sceneName)
    {
        // 씬 전환 전에 인벤토리 데이터 저장
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.StoreInventoryData();
        }

        // 씬 로드
        SceneManager.LoadScene(sceneName);
    }
} 