using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class StatUI : MonoBehaviour
{
    // 0 : health, 1 : stamina, ~~ 아래 함수 순서
    public List<TextMeshProUGUI> StatTextList;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI ExpText;
    public List<TextMeshProUGUI> SkillLevelTextList;

    private void Start()
    {
        UIEventManager.Instance.OnStatChanged += HealthChange;
        UIEventManager.Instance.OnStatChanged += StaminaChange;
        UIEventManager.Instance.OnStatChanged += ExpChange;
        UIEventManager.Instance.OnStatChanged += DamageChange;
        UIEventManager.Instance.OnStatChanged += MoveSpeedChange;
        UIEventManager.Instance.OnStatChanged += CriticalChanceChange;
        UIEventManager.Instance.OnStatChanged += CriticalDamageChange;
        UIEventManager.Instance.OnStatChanged += LevelChange;

        UIEventManager.Instance.OnStatChanged += SkillLevel;
    }

    public void HealthChange()
    {
        // "현재 체력 / 최대 체력
    }

    public void StaminaChange()
    {

    }

    public void DamageChange()
    {

    }

    public void MoveSpeedChange()
    {

    }

    public void CriticalChanceChange()
    {

    }

    public void CriticalDamageChange()
    {

    }
    public void ExpChange()
    {
        ExpText.text = "현재 경험치 / 현재레벨 최대 경험치";
    }

    public void LevelChange()
    {
        // LevelText.text = 플레이어 레벨 가져오기
    }

    public void SkillLevel()
    {
        for(int i = 0; i < SkillLevelTextList.Count; i++)
        {
            //SkillLevelTextList[i].text = $"Lv. {스킬레벨}";
        }
    }
}
