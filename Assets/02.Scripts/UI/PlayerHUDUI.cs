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

    public GameObject LevelUpImage;
    
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI ExpText;
    
    public List<Image> CooldownImageList;
    public List<TextMeshProUGUI> CooldownTextList;
    public List<bool> IsCooldownList = new List<bool>(4);

    private void Start()
    {
        Global.Instance.OnDataLoaded += Init;

        UIEventManager.Instance.OnStatChanged += ChangeStat;
        UIEventManager.Instance.OnCooldown += SKillCooldown;
        UIEventManager.Instance.OnExpGain += RefreshExpBar;
        UIEventManager.Instance.OnLevelUp += NewMaxExp;
        UIEventManager.Instance.OnLevelUp += OnLevelUpShowImage;
    }

    public void Init()
    {
        HealthBar.Initialize(PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].TotalStat);
        StaminaBar.Initialize(PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxStamina].TotalStat);
        ExpBar.Initialize(PlayerManager.Instance.PlayerLevel.ExpTable[PlayerManager.Instance.PlayerLevel.Level]);
        ExpBar.UpdateBar(PlayerManager.Instance.PlayerLevel.Experience);
        RefreshExpText();

        UIEventManager.Instance.OnStatChanged += ChangeStat;
    }

    // 체력 변동 후, 스태미나 변동 후 (차오를때도 계속), 경험치 변동 후 호출
    // MicroBar가 연속적인 처리는 렉걸린것처럼 늦게 반응하더라구요 스태미나 회복할때 이거 문제생기면 말씀해주세요 내일 해볼게요
    public void ChangeStat()
    {
        HealthBar.UpdateBar(PlayerManager.Instance.Player.Health, false);
        // 스태미나 부분
        StaminaBar.UpdateBar(PlayerManager.Instance.Player.Stamina, false);
        // 경험치 부분
        //ExpBar.UpdateBar(PlayerManager.Instance.현재 경험치);
    }

    public void RefreshExpBar(float currentExp)
    {
        ExpBar.UpdateBar(currentExp);
        // RefreshExpText();
    }
    
    public void NewMaxExp(int level, float maxExp)
    {
        ExpBar.SetNewMaxHP(maxExp);
        
    }
    
    private void RefreshExpText()
    {
        int percentage = Mathf.FloorToInt(100 * ExpBar.CurrentValue / ExpBar.MaxValue);
        ExpText.SetText($"{percentage}%");
    }

    public void SKillCooldown(int slot, float cooldownTime, float maxCooldownTime)
    {
        if (maxCooldownTime == 0)
        {
            CooldownImageList[slot].fillAmount = 0;
            return;
        }
        CooldownTextList[slot].gameObject.SetActive(true);
        CooldownImageList[slot].fillAmount = cooldownTime / maxCooldownTime;
        CooldownTextList[slot].text = cooldownTime.ToString("0.0");
        if (cooldownTime <= 0.1f)
        {
            CooldownTextList[slot].gameObject.SetActive(false);
        }
    }

    public IEnumerator Cooldown_coroution(int index)
    {
        // 스킬 슬롯은 0이 스킬 1인데 여긴 0이 기본공격
        IsCooldownList[index + 1] = true;
        CooldownImageList[index + 1].gameObject.SetActive(true);
        CooldownTextList[index + 1].gameObject.SetActive(true);
        //float Cooldown = 스킬 쿨타임;
        //float _time = Cooldown;
        //while (_time >= 0)
        //{
        //    // fillAmount 1에서 0으로 
        //    CooldownImageList[index + 1].fillAmount = _time / Cooldown;
        //    CooldownTextList[index + 1].text = _time.ToString("0.0");
        //    _time -= Time.deltaTime;
            yield return null;
        //}

        IsCooldownList[index + 1] = false;
        CooldownImageList[index + 1].gameObject.SetActive(false);
        CooldownTextList[index + 1].gameObject.SetActive(false);
    }

    private void OnLevelUpShowImage(int a, float b)
    {
        if (a == 1) return;
        InputManager.Instance._levelUpImage.SetActive(true);
    }
}
