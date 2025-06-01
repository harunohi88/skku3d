using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] private int _level;
    [SerializeField] private int _pointsGainPerLevel = 2;
    private UIEventManager _eventManager;
    private int _displayLevel => _level + 1;
    private float _experienceBonus =>
        PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.ExperienceGain].TotalStat;
    private List<float> ExpTable = new List<float>();
    private float _experience;
    [SerializeField] private int _statUpgradePoints = 0;

    private void Awake()
    {
        Global.Instance.OnDataLoaded += LoadData;
    }

    private void Start()
    {
        _eventManager = UIEventManager.Instance;
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
                _statUpgradePoints += _pointsGainPerLevel;
            }
            // Level Up Effect Execute once
            // UI에 레벨 표시
            StatSnapshot snapshot = PlayerManager.Instance.PlayerStat.CreateSnapshot();
            
            _eventManager.OnLevelUp?.Invoke();
            _eventManager.OnUpgradePointChange?.Invoke(_statUpgradePoints);
            Debug.Log($"Level Up: {_displayLevel}");
        }
        // UI에 경험치량 표시
    }

    public bool TryConsumePoints()
    {
        if (_statUpgradePoints <= 0)
        {
            return false;
        }

        _statUpgradePoints -= 1;
        _eventManager.OnUpgradePointChange?.Invoke(_statUpgradePoints);
        return true;
    }
}
