using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RuneShopUI : MonoBehaviour
{
    public List<Image> RuneImageList;
    public List<TextMeshProUGUI> RuneCostTextList;

    public TextMeshProUGUI RerollCostText;
    public List<Image> SoldoutImageList;    // 판매 완료 이미지리스트
    public List<Button> BuyButtonList;

    public RuneShop RuneShop;
    [SerializeField] private Tooltip _tooltip;  // 툴팁 참조

    private void Awake()
    {
        RuneShop = GetComponent<RuneShop>();
        RuneShop.OnRuneUpdated += UpdateShopItem;
        RuneShop.OnItemSoldout += SetSoldout;
        RuneShop.OnCreateRune += UnSetSoldout;
        RuneShop.OnReroll += UpdateRerollCost;

        // 각 버튼에 이벤트 트리거 추가
        for (int i = 0; i < BuyButtonList.Count; i++)
        {
            int index = i; // 클로저를 위한 로컬 변수
            EventTrigger trigger = BuyButtonList[i].gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = BuyButtonList[i].gameObject.AddComponent<EventTrigger>();

            // 마우스 진입 이벤트
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => { OnPointerEnter(index); });
            trigger.triggers.Add(enterEntry);

            // 마우스 이탈 이벤트
            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => { OnPointerExit(); });
            trigger.triggers.Add(exitEntry);
        }
    }

    private void OnPointerEnter(int index)
    {
        if (RuneShop.RuneList.Count > index)
        {
            Rune rune = RuneShop.RuneList[index];
            _tooltip.Show(rune.Name, rune.CurrentTier.ToString(), rune.RuneDescription, BuyButtonList[index].GetComponent<RectTransform>());
        }
    }

    private void OnPointerExit()
    {
        _tooltip.Hide();
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
        RerollCostText.text = currentRerollCost.ToString("N0");
    }

    public void UpdateShopItem(int index, Sprite runeIcon, int runeCost)
    {
        RuneImageList[index].sprite = runeIcon;
        RuneCostTextList[index].text = runeCost.ToString("N0");
    }

}
