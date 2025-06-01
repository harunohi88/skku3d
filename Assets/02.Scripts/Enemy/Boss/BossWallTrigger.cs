using UnityEngine;

public class BossWallTrigger : MonoBehaviour
{
    public Collider WallCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            WallCollider.enabled = true;
        }
    }
}
