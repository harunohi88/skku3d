using UnityEngine;
using TMPro;

public class LevelExpText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI CurrentExpText;
    [SerializeField] private TextMeshProUGUI NextLevelExpText;

    public void Start()
    {
        UIEventManager.Instance.OnExpGain += SetCurrentExp;
        UIEventManager.Instance.OnLevelUp += SetNextLevel;
    }
    
    public void SetCurrentExp(float currentExp)
    {
        CurrentExpText.text = $"{currentExp:F0}";
    }
    
    public void SetNextLevel(int level, float nextLevelExp)
    {
        LevelText.text = $"{level}";
        NextLevelExpText.text = $"{nextLevelExp:F0}";
    }
}
