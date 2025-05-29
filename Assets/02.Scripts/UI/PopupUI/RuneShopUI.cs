using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RuneShopUI : MonoBehaviour
{
    public List<Image> RuneImageList;
    public List<TextMeshProUGUI> RuneCostTextList;

    public TextMeshProUGUI RerollCostText;
    public List<Image> SoldoutImageList;    // 판매 완료 이미지리스트
    public List<Button> BuyButtonList;

    public RuneShop RuneShop;

    private void Awake()
    {
        RuneShop = GetComponent<RuneShop>();
        RuneShop.OnRuneUpdated += UpdateShopItem;
        RuneShop.OnItemSoldout += SetSoldout;
        RuneShop.OnCreateRune += UnSetSoldout;
        RuneShop.OnReroll += UpdateRerollCost;
    }

    public void SetSoldout(int index)
    {
        SoldoutImageList[index].gameObject.SetActive(true);
        BuyButtonList[index].interactable = false;
    }

    public void UnSetSoldout(int index)
    {
        SoldoutImageList[index].gameObject.SetActive(false);
        BuyButtonList[index].interactable = true;
    }

    public void UpdateRerollCost(int currentRerollCost)
    {
        RerollCostText.text = $"{currentRerollCost}";
    }

    public void UpdateShopItem(int index, Sprite runeIcon, int runeCost)
    {
        RuneImageList[index].sprite = runeIcon;
        RuneCostTextList[index].text = $"{runeCost}";
    }

}
