using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerLevel : MonoBehaviour
{
    public int Level;
    public List<float> ExpTable = new List<float>();
    
    [SerializeField] private int _pointsGainPerLevel = 2;
    
    private UIEventManager _eventManager;
    private int _displayLevel => Level + 1;
    private float _experienceBonus =>
        PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.ExperienceGain].TotalStat;
    public float Experience => _experience;
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
        _eventManager.OnExpGain?.Invoke(_experience);
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (_experience >= ExpTable[Level])
        {
            while (_experience >= ExpTable[Level])
            {
                _experience -= ExpTable[Level];
                ++Level;
                _statUpgradePoints += _pointsGainPerLevel;
            }
            // Level Up Effect Execute once
            // UI에 레벨 표시
            
            _eventManager.OnLevelUp?.Invoke(ExpTable[Level]);
            _eventManager.OnExpGain?.Invoke(_experience);
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
