using Microlight.MicroBar;
using TMPro;
using UnityEngine;

public class BossUIManager : MonoBehaviour
{
    public static BossUIManager Instance { get; private set; }
    [SerializeField] private MicroBar _bossHealthBar;
    [SerializeField] private TextMeshProUGUI _bossNameText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetBossUI(string BossName, int maxHealth)
    {
        _bossNameText.text = BossName;
        _bossHealthBar.Initialize(maxHealth);
        _bossHealthBar.SetNewMaxHP(maxHealth);
    }
    public void UPdateHealth(int currentHealth)
    {
        _bossHealthBar.UpdateBar(currentHealth);
    }
}
