using UnityEngine;
using System.Collections.Generic;

public class InventoryTest : MonoBehaviour
{
    [SerializeField] private BasicInventory _basicInventory;
    [SerializeField] private EquipInventory _equipInventory;
    [SerializeField] private List<ARune> _runePrefabsList; // 테스트용 룬 프리팹 목록

    private void Update()
    {
        // 1번 키를 눌러 랜덤 룬 추가
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_runePrefabsList.Count > 0)
            {
                int randomIndex = Random.Range(0, _runePrefabsList.Count);
                ARune randomRune = Instantiate(_runePrefabsList[randomIndex]);
                AddRuneToInventory(randomRune);
                Debug.Log($"Added rune: {randomRune.GetType().Name} (TID: {randomRune.TID})");
            }
        }

        // 2번 키를 눌러 인벤토리 내용 확인
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
        Debug.Log("=== 기본 인벤토리 내용 ===");
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
            Debug.Log("기본 인벤토리에 룬이 없습니다");
        }

        Debug.Log("=== 장비 인벤토리 내용 ===");

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
            Debug.Log("슬롯 0에 룬이 없습니다");
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
            Debug.Log("슬롯 1에 룬이 없습니다");
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
            Debug.Log("슬롯 2에 룬이 없습니다");
        }
    }
}
