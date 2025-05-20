using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("보스 기본공격 - Player HIT");
        }
    }
}
