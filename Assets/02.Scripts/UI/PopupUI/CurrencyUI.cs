using UnityEngine;
using TMPro;
public class CurrencyUI : MonoBehaviour
{
    public TextMeshProUGUI ShopGoldText;
    public TextMeshProUGUI InventoryGoldText;

    private void Start()
    {
        UpdateCurrency(CurrencyManager.Instance.CurrentGold);
        CurrencyManager.Instance.OnGoldChanged += UpdateCurrency;
    }

    private void UpdateCurrency(int gold)
    {
        ShopGoldText.text = gold.ToString("N0");
        InventoryGoldText.text = gold.ToString("N0");
    }
}
