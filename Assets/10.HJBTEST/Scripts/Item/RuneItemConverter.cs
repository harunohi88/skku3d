using UnityEngine;
using System.Collections.Generic;

namespace Rito.InventorySystem
{
    public static class RuneItemConverter
    {
        private static Dictionary<int, RuneItemData> _runeItemDataCache = new Dictionary<int, RuneItemData>();

        public static RuneItemData ConvertToItemData(RuneData runeData)
        {
            if (runeData == null) return null;

            if (!_runeItemDataCache.TryGetValue(runeData.TID, out RuneItemData itemData))
            {
                itemData = new RuneItemData(runeData);
                _runeItemDataCache[runeData.TID] = itemData;
            }

            return itemData;
        }

        public static void ClearCache()
        {
            _runeItemDataCache.Clear();
        }
    }
} 