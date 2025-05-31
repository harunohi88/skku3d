using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] public float Health;

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        UIEventManager.Instance.OnStatChanged?.Invoke();

        if (Health <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        Health += amount;
        Debug.Log($"Heal {amount}");
        Health = Mathf.Min(Health, PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].TotalStat);

        UIEventManager.Instance.OnStatChanged?.Invoke();
    }

    public void Die()
    {
        // Handle player death (e.g., play animation, respawn, etc.)
        Debug.Log("Player has died.");
    }

}
