using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class StatLevelUpUI : MonoBehaviour
{
    public GameObject UpgradeButtonPanel;
    public TextMeshProUGUI LevelUpPointText;

    public void UpgradeMaxHealth() => StatUpgrade(EStatType.MaxHealth);
    public void UpgradeStamina() => StatUpgrade(EStatType.MaxStamina);
    public void UpgradeAttackPower() => StatUpgrade(EStatType.AttackPower);
    public void UpgradeMoveSpeed() => StatUpgrade(EStatType.MoveSpeed);
    public void UpgradeCriticalChance() => StatUpgrade(EStatType.CriticalChance);
    public void UpgradeCriticalDamage() => StatUpgrade(EStatType.CriticalDamage);

    private void Start()
    {
        UIEventManager.Instance.OnLevelUp += OnLevelUp;
        UIEventManager.Instance.OnUpgradePointChange += PointRefresh;
    }

    private void PointRefresh(int statUpgradePoints)
    {
        // 포인트 표시하는 TextMesh 값 변경
        if (statUpgradePoints <= 0)
        {
            UpgradeButtonPanel.SetActive(false);
        }
    }

    private void OnLevelUp(float level)
    {
        // 레벨 표시하는 TextMesh 값 변경
        UpgradeButtonPanel.SetActive(true);
    }

    public void StatUpgrade(EStatType statType)
    {
        PlayerManager.Instance.PlayerStat.StatUpgrade((EStatType)statType);
    }

    // public void HealthStatUp()
    // {
    //     if (PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].CanLevelUp == false) return;
    //
    //     _levelUpPoint--;
    //     if(_levelUpPoint < 0) UpgradeButtonPanel.SetActive(false);
    //     PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].LevelUp();
    //     UIEventManager.Instance.OnStatChanged?.Invoke();
    // }
    //
    // public void StaminaStatUp()
    // {
    //     if (PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].CanLevelUp == false) return;
    //
    //     _levelUpPoint--;
    //     if (_levelUpPoint < 0) UpgradeButtonPanel.SetActive(false);
    //     PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxStamina].LevelUp();
    //     UIEventManager.Instance.OnStatChanged?.Invoke();
    // }
    //
    // public void DamageStatUp()
    // {
    //     if (PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].CanLevelUp == false) return;
    //
    //     _levelUpPoint--;
    //     if (_levelUpPoint < 0) UpgradeButtonPanel.SetActive(false);
    //     PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxStamina].LevelUp();
    //     UIEventManager.Instance.OnStatChanged?.Invoke();
    // }
    //
    // public void MoveSpeedStatUp()
    // {
    //     if (PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].CanLevelUp == false) return;
    //
    //     _levelUpPoint--;
    //     if (_levelUpPoint < 0) UpgradeButtonPanel.SetActive(false);
    //     PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MoveSpeed].LevelUp();
    //     UIEventManager.Instance.OnStatChanged?.Invoke();
    // }
    //
    // public void CriticalChanceStatUp()
    // {
    //     if (PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].CanLevelUp == false) return;
    //
    //     _levelUpPoint--;
    //     if (_levelUpPoint < 0) UpgradeButtonPanel.SetActive(false);
    //     PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.CriticalChance].LevelUp();
    //     UIEventManager.Instance.OnStatChanged?.Invoke();
    // }
    //
    // public void CriticalDamageStatUp()
    // {
    //     if (PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].CanLevelUp == false) return;
    //
    //     _levelUpPoint--;
    //     if (_levelUpPoint < 0) UpgradeButtonPanel.SetActive(false);
    //     PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.CriticalDamage].LevelUp();
    //     UIEventManager.Instance.OnStatChanged?.Invoke();
    // }
}
