using UnityEngine;
using UnityEditor;

public class RuneItem : MonoBehaviour
{
    // Basic Properties
    public int ID { get; private set; }
    public string Name { get; private set; }
    public string Tooltip { get; private set; }
    public Sprite IconSprite { get; private set; }
    public int Tier { get; private set; }
    public int RuneValue { get; private set; }
    public RuneData RuneData { get; private set; }

    // Countable Properties
    public int Amount { get; private set; }
    public int MaxAmount { get; private set; } = 99;
    public bool IsMax => Amount >= MaxAmount;
    public bool IsEmpty => Amount <= 0;

    public Inventory RuneInventory;

    public void Initialize(RuneData runeData, int tier, int amount = 1)
    {
        RuneData = runeData;
        ID = runeData.TID;
        Name = runeData.RuneName;
        Tier = tier;
        
        // 티어에 따라 계수 변환
        if(tier == 1)
        {
            RuneValue = runeData.TierList[0];
        }
        else if(tier == 2)
        {
            RuneValue = runeData.TierList[1];
        }
        else
        {
            RuneValue = runeData.TierList[2];
        }

        // 티어에 맞게 툴팁 변경
        string runeDescription = runeData.RuneDescription;
        runeDescription = runeDescription.Replace("N", RuneValue.ToString());
        Tooltip = runeDescription;

        // 스프라이트 로드
        string spritePath = $"Assets/04.Images/{runeData.RuneName}.png";
        IconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
        
        RuneInventory = DropTable.Instance.RuneInventory;
        

        SetAmount(amount);
    }

    public void SetAmount(int amount)
    {
        Amount = Mathf.Clamp(amount, 0, MaxAmount);
    }

    public int AddAmountAndGetExcess(int amount)
    {
        int nextAmount = Amount + amount;
        SetAmount(nextAmount);
        return (nextAmount > MaxAmount) ? (nextAmount - MaxAmount) : 0;
    }

    public RuneItem SeperateAndClone(int amount)
    {
        if(Amount <= 0) return null;

        if(amount > Amount)
            amount = Amount;

        Amount -= amount;

        // 새로운 게임오브젝트 생성
        GameObject newGo = new GameObject($"RuneItem_{Name}_Tier{Tier}_Split");
        RuneItem newItem = newGo.AddComponent<RuneItem>();
        newItem.Initialize(RuneData, Tier, amount);

        return newItem;
    }

    public bool Use()
    {
        if (IsEmpty) return false;
        
        // TODO: Implement rune usage logic
        Debug.Log($"Using rune: {RuneData.RuneName}");
        Amount--;
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (RuneInventory != null)
            {
                RuneInventory.Add(RuneData, Tier, Amount);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("RuneInventory reference is missing!");
            }
        }
    }
} 