using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgrade : MonoBehaviour
{
    public int MaxSkillLevel = 10;

    // 레벨 10이면
    // ex) 1000 2000 3000 4000 5000 6000 7000 8000 9000 0
    public List<int> SkillUpgradeCostList;

    public List<int> SkillLevelList = new List<int>() {1, 1, 1, 1};

    public Action<int, int> OnSkillUpgrade;
    public Action<int, int> OnSkillUpgradeText;
    public Action<int, bool> OnSkillMaxLevel;

    // TODO: 스킬을 참조하고 있어야 한다.


    /// <summary>
    /// 스킬 번호를 받아서 업그레이드 한다.
    /// </summary>
    /// <param name="skillNumber"></param>
    public void SkillUpgrde(int skillNumber)
    {
        if(skillNumber < 0 || skillNumber >= SkillLevelList.Count)
            return;
        
        int upgradeCost = SkillUpgradeCostList[SkillLevelList[skillNumber] - 1];

        if(CurrencyManager.Instance.TrySpendGold(upgradeCost))
        {
            // 스킬 레벨 증가
            SkillLevelList[skillNumber]++;

            // 스킬 레벨이 최대 레벨이면
            if(SkillLevelList[skillNumber] >= MaxSkillLevel)
            {
                OnSkillMaxLevel?.Invoke(skillNumber,false);
            }

            // 스킬 레벨 텍스트 업데이트
            OnSkillUpgrade?.Invoke(skillNumber, SkillLevelList[skillNumber]);
            // 스킬 레벨 업그레이드 코스트 텍스트 업데이트
            OnSkillUpgradeText?.Invoke(skillNumber, SkillUpgradeCostList[SkillLevelList[skillNumber] - 1]);
        }
        else
        {
            Debug.Log("골드가 부족해서 업그레이드 실패");
        }
    }
}
