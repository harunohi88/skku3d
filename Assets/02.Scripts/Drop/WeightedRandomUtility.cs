using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedItem<T>
{
    public T item;
    public float weight;
}

public class WeightedRandomUtility 
{
    /// <summary>
    /// 가중치 랜덤 선택
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="weightedItems"></param>
    /// <returns></returns>
    public static T GetWeightedRandom<T>(List<WeightedItem<T>> weightedItems)
    {
        // 만약 항목이 없으면 기본값 반환
        if (weightedItems.Count == 0)
        {
            return default(T);
        }

        // 모든 가중치를 더하여 총합을 구함
        float totalweight = 0f;
        foreach (var weightedItem in weightedItems)
        {
            totalweight += weightedItem.weight;
        }

        // 0부터 총합 사이의 랜덤 값을 생성
        float randomValue = Random.value * totalweight;

        // 랜던 값이 어느 범위에 속하는지 확인하여 항목 선택
        foreach (var weightedItem in weightedItems)
        {
            randomValue -= weightedItem.weight;
            if (randomValue <= 0)
            {
                return weightedItem.item;
            }
        }

        // 모든 항목이 0 이하인 경우 마지막 항목 반환
        return weightedItems[weightedItems.Count - 1].item;
    }
}
