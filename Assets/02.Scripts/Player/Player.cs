using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public float Health;
    public float Stamina;
    public float StaminaGain;
    public float StaminaRegenDelay;
    public float UIUpdateInterval = 0.1f;
    
    private float _staminaRegenTimer;
    private float _uiUpdateTimer;

    private void Awake()
    {
        PlayerManager.Instance.PlayerStat.OnDictionaryLoaded += Init;
        _staminaRegenTimer = StaminaRegenDelay; // 초기화 전에 회복하면 안돼서 넣은 임시방편
    }
    
    private void Init()
    {
        Health = PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].TotalStat;
        Stamina = PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxStamina].TotalStat;
    }

    private void Update()
    {
        if (_staminaRegenTimer > 0)
        {
            _staminaRegenTimer -= Time.deltaTime;
        }
        else
        {
            RegenerateStamina();
        }
    }
    
    private void RegenerateStamina()
    {
        Stamina += StaminaGain * Time.deltaTime;
        Stamina = Mathf.Min(Stamina, PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxStamina].TotalStat);
        _uiUpdateTimer -= Time.deltaTime;
        if (_uiUpdateTimer <= 0f)
        {
            UIEventManager.Instance.OnStatChanged?.Invoke();
            _uiUpdateTimer = UIUpdateInterval;
        }
    }

    public bool TryUseStamina(float amount)
    {
        if (Stamina < amount)
        {
            return false;
        }
        Stamina -= amount;
        _staminaRegenTimer = StaminaRegenDelay;
        UIEventManager.Instance.OnStatChanged?.Invoke();
        return true;
    }
    
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
