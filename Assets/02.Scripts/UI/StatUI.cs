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

    private Dictionary<EStatType, TextMeshProUGUI> _textDictionary;
    
    private void Start()
    {
        _textDictionary = new Dictionary<EStatType, TextMeshProUGUI>();
        foreach (StatText statText in StatTextList)
        {
            _textDictionary[statText.StatType] = statText.Text;
        }

        UIEventManager.Instance.OnDisplayStatChanged += RefreshStatTexts;
    }

    private void RefreshStatTexts(StatSnapshot statSnapshot)
    {
        foreach (KeyValuePair<EStatType, float> stat in statSnapshot.TotalStats)
        {
            if (_textDictionary.TryGetValue(stat.Key, out TextMeshProUGUI text))
            {
                text.text = $"{stat.Value:F0}";
            }
        }
    }
}
