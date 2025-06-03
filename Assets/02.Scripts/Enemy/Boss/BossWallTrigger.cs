using UnityEngine;
using UnityEngine.SceneManagement;

public class BossWallTrigger : MonoBehaviour
{
    public Collider WallCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Stage1_Boss"))
            {
                AudioManager.Instance.PlayBGM(1);
            }
            if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Stage2_Boss"))
            {
                AudioManager.Instance.PlayBGM(2);
            }
            if( SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Stage3_Boss"))
            {
                AudioManager.Instance.PlayBGM(3);
            }
            GetComponent<BoxCollider>().enabled = false;
            WallCollider.enabled = true;
        }
    }
}
