using UnityEngine;
using System.Collections.Generic;

public class InventoryManager2 : MonoBehaviour
{
    private static InventoryManager2 _instance;
    public static InventoryManager2 Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InventoryManager2>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("InventoryManager2");
                    _instance = go.AddComponent<InventoryManager2>();
                }
            }
            return _instance;
        }
    }

    private BasicAllInventory _basicAllInventory;
    private BasicInventory _tier1Inventory;
    private BasicInventory _tier2Inventory;
    private BasicInventory _tier3Inventory;
    private EquipInventory _equipInventory;

    // 인벤토리 데이터 저장용
    private List<InventoryItem> _storedBasicAllItems;
    private List<InventoryItem> _storedTier1Items;
    private List<InventoryItem> _storedTier2Items;
    private List<InventoryItem> _storedTier3Items;
    private Dictionary<int, InventoryItem> _storedEquipItems; // 슬롯 인덱스를 키로 사용

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // 저장소 초기화
        _storedBasicAllItems = new List<InventoryItem>();
        _storedTier1Items = new List<InventoryItem>();
        _storedTier2Items = new List<InventoryItem>();
        _storedTier3Items = new List<InventoryItem>();
        _storedEquipItems = new Dictionary<int, InventoryItem>();
    }

    public void RegisterInventory(BasicAllInventory inventory)
    {
        _basicAllInventory = inventory;
        RestoreInventoryData(_basicAllInventory, _storedBasicAllItems);
    }

    public void RegisterInventory(BasicInventory inventory, int tier)
    {
        switch (tier)
        {
            case 1:
                _tier1Inventory = inventory;
                RestoreInventoryData(_tier1Inventory, _storedTier1Items);
                break;
            case 2:
                _tier2Inventory = inventory;
                RestoreInventoryData(_tier2Inventory, _storedTier2Items);
                break;
            case 3:
                _tier3Inventory = inventory;
                RestoreInventoryData(_tier3Inventory, _storedTier3Items);
                break;
        }
    }

    public void RegisterInventory(EquipInventory inventory)
    {
        _equipInventory = inventory;
        RestoreEquipInventoryData();
    }

    private void RestoreInventoryData(BaseInventory inventory, List<InventoryItem> storedItems)
    {
        if (inventory == null || storedItems == null) return;

        // 저장된 아이템 복원
        for (int i = 0; i < storedItems.Count; i++)
        {
            if (storedItems[i] != null)
            {
                inventory.AddItem(storedItems[i].Rune, storedItems[i].Quantity);
            }
        }
    }

    private void RestoreEquipInventoryData()
    {
        if (_equipInventory == null || _storedEquipItems == null) return;

        // 저장된 장비 아이템을 원래 슬롯 위치에 복원
        foreach (var kvp in _storedEquipItems)
        {
            if (kvp.Value != null)
            {
                _equipInventory.AddItemToSlot(kvp.Value.Rune, kvp.Key, kvp.Value.Quantity);
            }
        }
    }

    public void StoreInventoryData()
    {
        if (_basicAllInventory != null)
            _storedBasicAllItems = new List<InventoryItem>(_basicAllInventory.GetItemList());
        
        if (_tier1Inventory != null)
            _storedTier1Items = new List<InventoryItem>(_tier1Inventory.GetItemList());
        
        if (_tier2Inventory != null)
            _storedTier2Items = new List<InventoryItem>(_tier2Inventory.GetItemList());
        
        if (_tier3Inventory != null)
            _storedTier3Items = new List<InventoryItem>(_tier3Inventory.GetItemList());
        
        if (_equipInventory != null)
        {
            _storedEquipItems.Clear();
            var itemList = _equipInventory.GetItemList();
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i] != null)
                {
                    _storedEquipItems[i] = itemList[i];
                }
            }
        }
    }

    public BasicAllInventory GetBasicAllInventory()
    {
        return _basicAllInventory;
    }

    public BasicInventory GetTierInventory(int tier)
    {
        switch (tier)
        {
            case 1:
                return _tier1Inventory;
            case 2:
                return _tier2Inventory;
            case 3:
                return _tier3Inventory;
            default:
                return null;
        }
    }

    public EquipInventory GetEquipInventory()
    {
        return _equipInventory;
    }
} 