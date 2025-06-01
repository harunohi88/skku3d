using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgrade : MonoBehaviour
{
    public int MaxSkillLevel = 10;

    public List<int> SkillLevelList = new List<int>() {1, 1, 1, 1};
    public List<int> SkillUpgradeCostList;

    public Action<int, int> OnSkillUpgrade;
    public Action<int, int> OnSkillUpgradeText;
    public Action<int, bool> OnSkillMaxLevel;

    private int _skillUpgradeIncrease = 200;


    private const int SKILL_UPGRADE_COST_TID = 10000;

    private bool _isDataLoaded = false;


    private void Start()
    {
        SkillUpgradeCostList = new List<int>(4) { 500, 500, 500, 500 };

    }

    // TODO: 스킬을 참조하고 있어야 한다.


    /// <summary>
    /// 스킬 번호를 받아서 업그레이드 한다.
    /// </summary>
    /// <param name="skillNumber"></param>
    public void SkillUpgrde(int skillNumber)
    {
        if(!_isDataLoaded)
        {
            CurrencyData currencyData = DataTable.Instance.GetCurrencyData(SKILL_UPGRADE_COST_TID);
            SkillUpgradeCostList[0] = currencyData.BaseAmount;
            SkillUpgradeCostList[1] = currencyData.BaseAmount;
            SkillUpgradeCostList[2] = currencyData.BaseAmount;
            SkillUpgradeCostList[3] = currencyData.BaseAmount;

            _skillUpgradeIncrease = currencyData.AddAmount;
            _isDataLoaded = true;
        }
        if(skillNumber < 0 || skillNumber >= SkillLevelList.Count)
            return;
        
        int upgradeCost = SkillUpgradeCostList[skillNumber];

        if(CurrencyManager.Instance.TrySpendGold(upgradeCost))
        {
            // 스킬 레벨 증가
            SkillLevelList[skillNumber]++;
            SkillUpgradeCostList[skillNumber] += _skillUpgradeIncrease;

            //스킬 렙업

            // 스킬 레벨이 최대 레벨이면
            if(SkillLevelList[skillNumber] >= MaxSkillLevel)
            {
                OnSkillMaxLevel?.Invoke(skillNumber,false);
            }

            // 스킬 레벨 텍스트 업데이트
            OnSkillUpgrade?.Invoke(skillNumber, SkillLevelList[skillNumber]);
            // 스킬 레벨 업그레이드 코스트 텍스트 업데이트
            OnSkillUpgradeText?.Invoke(skillNumber, SkillUpgradeCostList[skillNumber]);
        }
        else
        {
            Debug.Log("골드가 부족해서 업그레이드 실패");
        }
    }
}
