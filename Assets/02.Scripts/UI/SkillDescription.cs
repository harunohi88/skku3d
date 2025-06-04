using System;
using UnityEngine;
using TMPro;

public class SkillDescription : MonoBehaviour
{
    public string Description;
    public TextMeshProUGUI SkillLevelText;
    public TextMeshProUGUI SkillDescriptionText;
    
    public void SetSkillDescription(int level, float multiplier)
    {
        SkillDescriptionText.text = Description.Replace("{N}", (100f * multiplier).ToString("F0"));
        SkillLevelText.text = $"Lv. {level}";
    }
}