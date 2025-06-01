using UnityEngine;

public class BossWallTrigger : MonoBehaviour
{
    public Collider WallCollider;
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<BoxCollider>().enabled = false;
        WallCollider.enabled = true;
    }
}
