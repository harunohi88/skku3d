using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByName("Stage3_Boss").buildIndex)
            {
                SceneManager.LoadScene(SceneManager.GetSceneByName("SUCCESS Scene").buildIndex);
                return;

            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            // 인벤토리 정보 저장
            InventoryManager.Instance.StoreInventoryData();

            GameManager.Instance.GoToNextStage();
        }
    }
}
