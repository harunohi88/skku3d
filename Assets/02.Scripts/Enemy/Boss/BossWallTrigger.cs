using UnityEngine;
using UnityEngine.SceneManagement;

public class BossWallTrigger : MonoBehaviour
{
    public Collider WallCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                AudioManager.Instance.PlayBGM(1);
            }
            if(SceneManager.GetActiveScene().buildIndex >= 4)
            {
                AudioManager.Instance.PlayBGM(SceneManager.GetActiveScene().buildIndex - 1);
            }
            GetComponent<BoxCollider>().enabled = false;
            WallCollider.enabled = true;
        }
    }
}
