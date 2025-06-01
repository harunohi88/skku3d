using UnityEngine;
using TMPro;

public class StatLevelUpUI : MonoBehaviour
{
    public GameObject UpgradeButtonPanel;
    public TextMeshProUGUI LevelUpPointText;
    private int _levelUpPoint;

    private void Start()
    {
        _levelUpPoint = 0;
        UIEventManager.Instance.OnLevelUp += OnLevelUp;
    }

    private void OnLevelUp()
    {
        _levelUpPoint += 2;
        UpgradeButtonPanel.SetActive(true);
    }

    public void HealthStatUp()
    {
        if (PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].CanLevelUp == false) return;

        _levelUpPoint--;
        if(_levelUpPoint < 0) UpgradeButtonPanel.SetActive(false);
        PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].LevelUp();
        UIEventManager.Instance.OnStatChanged?.Invoke();
    }

    public void StaminaStatUp()
    {
        if (PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].CanLevelUp == false) return;

        _levelUpPoint--;
        if (_levelUpPoint < 0) UpgradeButtonPanel.SetActive(false);
        PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxStamina].LevelUp();
        UIEventManager.Instance.OnStatChanged?.Invoke();
    }

    public void DamageStatUp()
    {
        if (PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].CanLevelUp == false) return;

        _levelUpPoint--;
        if (_levelUpPoint < 0) UpgradeButtonPanel.SetActive(false);
        PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxStamina].LevelUp();
        UIEventManager.Instance.OnStatChanged?.Invoke();
    }

    public void MoveSpeedStatUp()
    {
        if (PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].CanLevelUp == false) return;

        _levelUpPoint--;
        if (_levelUpPoint < 0) UpgradeButtonPanel.SetActive(false);
        PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MoveSpeed].LevelUp();
        UIEventManager.Instance.OnStatChanged?.Invoke();
    }

    public void CriticalChanceStatUp()
    {
        if (PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].CanLevelUp == false) return;

        _levelUpPoint--;
        if (_levelUpPoint < 0) UpgradeButtonPanel.SetActive(false);
        PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.CriticalChance].LevelUp();
        UIEventManager.Instance.OnStatChanged?.Invoke();
    }

    public void CriticalDamageStatUp()
    {
        if (PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].CanLevelUp == false) return;

        _levelUpPoint--;
        if (_levelUpPoint < 0) UpgradeButtonPanel.SetActive(false);
        PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.CriticalDamage].LevelUp();
        UIEventManager.Instance.OnStatChanged?.Invoke();
    }
}
