using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Damage damage = new Damage();
            damage.Value = BossAIManager.Instance.GetPatternData(0).Damage;
            PlayerManager.Instance.Player.TakeDamage(damage);
        }
    }
}
