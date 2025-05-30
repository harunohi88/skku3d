using UnityEngine;
using Microlight.MicroBar;

public class PlayerHUDUI : MonoBehaviour
{
    public MicroBar HealthBar;
    public MicroBar StaminaBar;
    public MicroBar ExpBar;

    public MicroBar Skill1CooldownBar;
    public MicroBar Skill2CooldownBar;
    public MicroBar Skill3CooldownBar;

    private void Start()
    {
        HealthBar.Initialize(PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].TotalStat);
        StaminaBar.Initialize(PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxStamina].TotalStat);
        // 경험치 초기화

        //Skill1CooldownBar.Initialize(PlayerManager.Instance.PlayerSkill)
    }
}
