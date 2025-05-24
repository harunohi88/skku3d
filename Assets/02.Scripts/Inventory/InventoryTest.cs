using UnityEngine;
using System.Collections.Generic;

public class InventoryTest : MonoBehaviour
{
    [SerializeField] private BasicInventory _basicInventory;
    [SerializeField] private EquipInventory _equipInventory;
    [SerializeField] private List<ARune> _runePrefabs; // List of rune prefabs to test with

    private void Update()
    {
        // Press 1 to add random rune
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_runePrefabs.Count > 0)
            {
                int randomIndex = Random.Range(0, _runePrefabs.Count);
                ARune randomRune = Instantiate(_runePrefabs[randomIndex]);
                AddRuneToInventory(randomRune);
                Debug.Log($"Added rune: {randomRune.GetType().Name} (TID: {randomRune.TID})");
            }
        }

        // Press 2 to check inventory contents
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CheckInventoryContents();
        }
    }

    public void AddRuneToInventory(ARune rune)
    {
        _basicInventory.AddItem(rune, 1);
    }

    private void CheckInventoryContents()
    {
        Debug.Log("=== Basic Inventory Contents ===");
        var runeList = _basicInventory.GetRuneList();
        if (runeList != null)
        {
            foreach (var rune in runeList)
            {
                if (rune != null)
                {
                    Debug.Log($"Rune: {rune.GetType().Name} (TID: {rune.TID})");
                    Debug.Log($"Description: {rune.RuneDescription}");
                    Debug.Log($"Tier Value: {rune.TierValue}");
                    Debug.Log("-------------------");
                }
            }
        }
        else
        {
            Debug.Log("No runes in basic inventory");
        }

        Debug.Log("=== Equip Inventory Contents ===");

        var test = _equipInventory.GetRuneAtSlot(0);
        if(test != null)
        {
            Debug.Log($"Rune: {test.GetType().Name} (TID: {test.TID})");
            Debug.Log($"Description: {test.RuneDescription}");
            Debug.Log($"Tier Value: {test.TierValue}");
            Debug.Log("-------------------");
        }
        else
        {
            Debug.Log("No rune in slot 0");
        }
        var test2 = _equipInventory.GetRuneAtSlot(1);
        if(test2 != null)
        {
            Debug.Log($"Rune: {test2.GetType().Name} (TID: {test2.TID})");
            Debug.Log($"Description: {test2.RuneDescription}");
            Debug.Log($"Tier Value: {test2.TierValue}");
            Debug.Log("-------------------");
        }
        else
        {
            Debug.Log("No rune in slot 1");
        }
        var test3 = _equipInventory.GetRuneAtSlot(2);
        if(test3 != null)
        {
            Debug.Log($"Rune: {test3.GetType().Name} (TID: {test3.TID})");
            Debug.Log($"Description: {test3.RuneDescription}");
            Debug.Log($"Tier Value: {test3.TierValue}");
            Debug.Log("-------------------");
        }
        else
        {
            Debug.Log("No rune in slot 2");
        }
    }
}
