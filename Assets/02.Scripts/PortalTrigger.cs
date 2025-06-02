using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            // 인벤토리 정보 저장
            InventoryManager.Instance.StoreInventoryData();

            GameManager.Instance.GoToNextStage();
        }
    }
}
