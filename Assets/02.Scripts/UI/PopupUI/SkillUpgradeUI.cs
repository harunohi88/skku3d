using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUpgradeUI : MonoBehaviour
{
    public SkillUpgrade SkillUpgrade;

    public List<Button> SkillButtonList;
    public List<TextMeshProUGUI> SkillCostTextList;
    public List<TextMeshProUGUI> SkillLevelTextList;
    public List<SkillDescription> SkillDescriptionList;
    public TextMeshProUGUI Skill1DescriptionText;
    public TextMeshProUGUI Skill2DescriptionText;
    public TextMeshProUGUI Skill3DescriptionText;
    public TextMeshProUGUI Skill4DescriptionText;

    private void Start()
    {
        // SkillUpgrade.OnSkillUpgrade += UpdateSkillLevelText;
        SkillUpgrade.OnSkillUpgradeText += UpdateSkillCostText;
        SkillUpgrade.OnSkillMaxLevel += SetInteractableButton;
        UIEventManager.Instance.OnSkillDescriptionChanged += RefreshDescription;

        for(int i=0; i<SkillUpgrade.SkillUpgradeCostList.Count; i++)
        {
            UpdateSkillCostText(i, SkillUpgrade.SkillUpgradeCostList[i]);
        }
    }

    public void UpdateSkillLevelText(int skillNumber, int level)
    {
        SkillLevelTextList[skillNumber].text = $"Lv.{level}";
    }

    public void UpdateSkillCostText(int skillNumber, int upgradeCost)
    {
        SkillCostTextList[skillNumber].text = $"{upgradeCost}";
    }

    public void SetInteractableButton(int skillNumber, bool isInteractable)
    {
        SkillButtonList[skillNumber].interactable = isInteractable;
    }

    private void RefreshDescription(int index, int level, float multiplier)
    {
        SkillDescriptionList[index].SetSkillDescription(level, multiplier);
    }
    
}
