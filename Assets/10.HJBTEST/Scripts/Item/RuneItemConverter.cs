using UnityEngine;
using System.Collections.Generic;

namespace Rito.InventorySystem
{
    public static class RuneItemConverter
    {
        private static Dictionary<(int, int), RuneItem> _runeItemCache = new Dictionary<(int, int), RuneItem>();

        public static RuneItem ConvertToRuneItem(RuneData runeData, int tier)
        {
            if (runeData == null) return null;

            var key = (runeData.TID, tier);

            if (!_runeItemCache.TryGetValue(key, out RuneItem runeItem))
            {
                runeItem = new RuneItem(runeData, tier);
                _runeItemCache[key] = runeItem;
            }

            return runeItem;
        }

        public static void ClearCache()
        {
            _runeItemCache.Clear();
        }
    }
}