using UnityEngine;
using System.Collections.Generic;

public class PlayerLevel : MonoBehaviour
{
    private int _displayLevel => _level + 1;

    private float _experienceBonus =>
        PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.ExperienceGain].TotalStat;
    private int _level;
    private List<float> ExpTable = new List<float>();
    
    private float _experience;

    private void Awake()
    {
        Global.Instance.OnDataLoaded += LoadData;
    }

    private void LoadData()
    {
        ReadOnlyList<PlayerExperienceData> data = DataTable.Instance.GetPlayerExperienceDataList();

        foreach (PlayerExperienceData experienceData in data)
        {
            ExpTable.Add(experienceData.ExpToNextLevel);
        }
    }
    
    public void GainExperience(float experience)
    {
        Debug.Log($"Gain Experience: {experience}");
        
        _experience += (1f + _experienceBonus) * experience;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (_experience >= ExpTable[_level])
        {
            while (_experience >= ExpTable[_level])
            {
                _experience -= ExpTable[_level];
                ++_level;
            }
            // Level Up Effect Execute once
            // UI에 레벨 표시
            Debug.Log($"Level Up: {_displayLevel}");
        }
        // UI에 경험치량 표시
    }
}
