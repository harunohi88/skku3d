using UnityEngine;
using System.Collections.Generic;

namespace Rito.InventorySystem
{
    public static class RuneItemConverter
    {
        private static Dictionary<(int, int), RuneItemData> _runeItemDataCache = new Dictionary<(int, int), RuneItemData>();

        public static RuneItemData ConvertToItemData(RuneData runeData, int tier)
        {
            if (runeData == null) return null;

            var key = (runeData.TID, tier);

            if (!_runeItemDataCache.TryGetValue(key, out RuneItemData itemData))
            {
                itemData = new RuneItemData(runeData, tier);
                _runeItemDataCache[key] = itemData;
            }

            return itemData;
        }

        public static void ClearCache()
        {
            _runeItemDataCache.Clear();
        }
    }
}