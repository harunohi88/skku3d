using UnityEngine;
using System.Collections.Generic;

public static class RuneItemConverter
{
    private static Dictionary<(int, int), RuneItem> _runeItemCache = new Dictionary<(int, int), RuneItem>();

    public static RuneItem ConvertToRuneItem(RuneData runeData, int tier)
    {
        if (runeData == null) return null;

        var key = (runeData.TID, tier);

        if (!_runeItemCache.TryGetValue(key, out RuneItem runeItem))
        {
            GameObject go = new GameObject($"RuneItem_{runeData.RuneName}_Tier{tier}");
            runeItem = go.AddComponent<RuneItem>();
            runeItem.Initialize(runeData, tier);
            _runeItemCache[key] = runeItem;
        }

        return runeItem;
    }

    public static void ClearCache()
    {
        _runeItemCache.Clear();
    }
}