using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class StatText
{
    public EStatType StatType;
    public TextMeshProUGUI Text;
}

public class StatUI : MonoBehaviour
{
    [SerializeField] private List<StatText> StatTextList;
    [SerializeField] private TextMeshProUGUI CurrentHealthText;

    private Dictionary<EStatType, TextMeshProUGUI> _textDictionary;
    
    private void Start()
    {
        _textDictionary = new Dictionary<EStatType, TextMeshProUGUI>();
        foreach (StatText statText in StatTextList)
        {
            _textDictionary[statText.StatType] = statText.Text;
        }

        UIEventManager.Instance.OnDisplayStatChanged += RefreshStatTexts;
        UIEventManager.Instance.OnCurrentHealthChanged += RefreshCurrentHealthText;
    }

    private void RefreshCurrentHealthText(float currentHealth)
    {
        if (CurrentHealthText != null)
        {
            CurrentHealthText.text = $"{currentHealth:F0}";
        }
    }

    private void RefreshStatTexts(StatSnapshot statSnapshot)
    {
        foreach (KeyValuePair<EStatType, float> stat in statSnapshot.TotalStats)
        {
            if (_textDictionary.TryGetValue(stat.Key, out TextMeshProUGUI text))
            {
                if (stat.Key == EStatType.CriticalChance || stat.Key == EStatType.CriticalDamage)
                {
                    text.text = $"{stat.Value * 100:F0}%";
                }
                else if (stat.Key == EStatType.MoveSpeed)
                {
                    text.text = $"{stat.Value:F2}";
                }
                else
                {
                    text.text = $"{stat.Value:F0}";
                }
            }
        }
    }
}
