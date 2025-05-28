using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] public float Health;

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        if (Health <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        Health += amount;
        Health = Mathf.Min(Health, PlayerManager.Instance.PlayerStat.MaxHealth);
    }

    public void Die()
    {
        // Handle player death (e.g., play animation, respawn, etc.)
        Debug.Log("Player has died.");
    }

}
