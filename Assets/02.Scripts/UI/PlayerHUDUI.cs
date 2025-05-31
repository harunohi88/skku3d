using UnityEngine;
using Microlight.MicroBar;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PlayerHUDUI : MonoBehaviour
{
    public MicroBar HealthBar;
    public MicroBar StaminaBar;
    public MicroBar ExpBar;

    public List<Image> CooldownImageList;
    public List<TextMeshProUGUI> CooldownTextList;

    private void Start()
    {
        Global.Instance.OnDataLoaded += Init;
    }

    public void Init()
    {
        HealthBar.Initialize(PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].TotalStat);
        StaminaBar.Initialize(PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxStamina].TotalStat);
        // 경험치 초기화

        UIEventManager.Instance.OnStatChanged += ChangeStat;
    }

    public void ChangeStat()
    {
        HealthBar.UpdateBar(PlayerManager.Instance.Player.Health);
        // StaminaBar.UpdateBar
    }

    public void SKillCooldown()
    {

    }

    //public IEnumerator Cooldown_coroutine(int index)
    //{
    //    float time = 0f;
    //    yield return 
    //}
}
