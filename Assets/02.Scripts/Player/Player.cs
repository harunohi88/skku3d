using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : BehaviourSingleton<Player>, IDamageable
{
    public float Health;
    public float Stamina;
    public float StaminaGain;
    public float StaminaRegenDelay;
    public float UIUpdateInterval = 0.1f;
    public Animator Animator;
    
    private float _staminaRegenTimer;
    private float _uiUpdateTimer;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        PlayerManager.Instance.PlayerStat.OnDictionaryLoaded += Init;
        _staminaRegenTimer = StaminaRegenDelay; // 초기화 전에 회복하면 안돼서 넣은 임시방편
    }
    
    private void Init()
    {
        Health = PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].TotalStat;
        Stamina = PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxStamina].TotalStat;
        UIEventManager.Instance.OnStatChanged?.Invoke();
        UIEventManager.Instance.OnCurrentHealthChanged?.Invoke(Health);
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
        if (Health <= 0) return;
        
        Health -= damage.Value;
        UIEventManager.Instance.OnStatChanged?.Invoke();
        UIEventManager.Instance.OnCurrentHealthChanged?.Invoke(Health);

        HUDUI.Instance.ShowDamageVignette();

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
        UIEventManager.Instance.OnCurrentHealthChanged?.Invoke(Health);
    }

    public void Die()
    {
        Debug.Log("Player has died.");

        InputManager.Instance.TurnOff = true;
        GameObject.FindGameObjectWithTag("PlayerSpawnPoint").transform.position = transform.position;
        Animator.SetTrigger("Death");
        float deathTime = Animator.GetCurrentAnimatorStateInfo(0).length;
        InputManager.Instance.SetEveryPanelOff();
        StartCoroutine(DieCoroutine(deathTime));
    }

    private IEnumerator DieCoroutine(float deathTime)
    {
        yield return new WaitForSeconds(deathTime + 1f);

        Time.timeScale = 0f;
        SceneManager.LoadScene("DEFEAT Scene", LoadSceneMode.Additive);
    }

}
