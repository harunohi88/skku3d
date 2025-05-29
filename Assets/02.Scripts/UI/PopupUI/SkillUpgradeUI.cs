using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUpgradeUI : MonoBehaviour
{
    public SkillUpgrade skillUpgrade;

    public List<Button> skillButtonList;
    public List<TextMeshProUGUI> skillCostTextList;
    public List<TextMeshProUGUI> skillLevelTextList;
    
    public TextMeshProUGUI Skill1DescriptionText;
    public TextMeshProUGUI Skill2DescriptionText;
    public TextMeshProUGUI Skill3DescriptionText;
    public TextMeshProUGUI Skill4DescriptionText;

    private void Start()
    {
        skillUpgrade.OnSkillUpgrade += UpdateSkillLevelText;
        skillUpgrade.OnSkillUpgradeText += UpdateSkillCostText;
        skillUpgrade.OnSkillMaxLevel += SetInteractableButton;
    }

    public void UpdateSkillLevelText(int skillNumber, int level)
    {
        skillLevelTextList[skillNumber].text = $"Lv.{level}";
    }

    public void UpdateSkillCostText(int skillNumber, int upgradeCost)
    {
        skillCostTextList[skillNumber].text = $"{upgradeCost}";
    }

    public void SetInteractableButton(int skillNumber, bool isInteractable)
    {
        skillButtonList[skillNumber].interactable = isInteractable;
    }

    
}
