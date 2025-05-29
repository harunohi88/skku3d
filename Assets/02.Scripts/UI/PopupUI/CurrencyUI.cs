using UnityEngine;
using TMPro;
public class CurrencyUI : MonoBehaviour
{
    public TextMeshProUGUI ShopGoldText;
    public TextMeshProUGUI InventoryGoldText;

    private void Start()
    {
        CurrencyManager.Instance.OnGoldChanged += UpdateCurrency;
    }

    private void UpdateCurrency(int gold)
    {
        ShopGoldText.text = gold.ToString();
        InventoryGoldText.text = gold.ToString();
    }
}
